using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reactive;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Rogue.MetroFire.CampfireClient.Infrastructure;
using Rogue.MetroFire.CampfireClient.Serialization;
 
namespace Rogue.MetroFire.CampfireClient
{

	public class CampfireApi : ICampfireApi
	{
		private readonly ISettings _settings;
		private LoginInfo _loginInfo;
		private readonly IDictionary<SerializerEntry, XmlSerializer> _xmlSerializers = new Dictionary<SerializerEntry, XmlSerializer>();
		private static int _defaultTimeout;
		private string _cookie;
		public const string UserAgent = "metro fire - https://github.com/ArildF/MetroFire";
		private const string CampfireBaseUri = "https://{0}.campfirenow.com";

		public CampfireApi(ISettings settings)
		{
			_settings = settings;
			var serializers = XmlSerializer.FromTypes(new[] {typeof (Room), typeof (User), typeof (Account), typeof(Message), typeof(Upload)});
			_xmlSerializers[new SerializerEntry(typeof (Room))] = serializers[0];
			_xmlSerializers[new SerializerEntry(typeof (User))] = serializers[1];
			_xmlSerializers[new SerializerEntry(typeof (Account))] = serializers[2];
			_xmlSerializers[new SerializerEntry(typeof (Message))] = serializers[3];
			_xmlSerializers[new SerializerEntry(typeof (Upload))] = serializers[4];

			_xmlSerializers[new SerializerEntry("messages", typeof(Message[]))] = 
				new XmlSerializer(typeof(Message[]), new XmlRootAttribute("messages"));

			_xmlSerializers[new SerializerEntry("rooms", typeof(Room[]))] = 
				new XmlSerializer(typeof(Room[]), new XmlRootAttribute("rooms"));

			_defaultTimeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;
		}

		internal string Cookie
		{
			get { return _cookie; }
		}

		public Account GetAccountInfo()
		{
			try
			{
				return Get<Account>("/account.xml");
			}
			catch (WebException e)
			{
				if (e.Response == null)
				{
					throw;
				}

				if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized)
				{
					return null;
				}
				throw;
			}

		}

		public Room[] ListRooms()
		{
			var roomArray = Get<Room[]>("rooms.xml", "rooms");
			return roomArray;
		}

		public Unit Join(int id)
		{
			string relativeUri = String.Format("room/{0}/join.xml", id);
			Post<NoResponse>(relativeUri);

			return Unit.Default;
		}

		public Message Speak(int id, string text)
		{
			string relativeUri = String.Format("room/{0}/speak.xml", id);
			var msg = new Message {Type = MessageType.TextMessage, Body = text};

			return Post<Message>(relativeUri, msg, HttpStatusCode.Created);
		}

		public void SetLoginInfo(LoginInfo loginInfo)
		{
			_loginInfo = loginInfo;
		}

		public Room[] ListPresence()
		{
			var roomArray = Get<Room[]>("presence.xml", "rooms");
			return roomArray;
		}

		public void Leave(int id)
		{
			var relativeUri = String.Format("room/{0}/leave.xml", id);
			Post<NoResponse>(relativeUri);
		}

		public Room GetRoom(int roomId)
		{
			var relativeUri = String.Format("room/{0}.xml", roomId);
			return Get<Room>(relativeUri);
		}

		public User GetUser(int userId)
		{
			var relativeUri = String.Format("users/{0}.xml", userId);
			return Get<User>(relativeUri);
		}

		public Unit LeaveRoom(int id)
		{
			string relativeUri = String.Format("room/{0}/leave.xml", id);
			Post<NoResponse>(relativeUri);
			return Unit.Default;
		}

		public Upload GetUpload(int roomId, int uploadMessageId)
		{
			var relativeUri = String.Format("room/{0}/messages/{1}/upload.xml", roomId, uploadMessageId);
			return Get<Upload>(relativeUri);
		}

		public Unit DownloadFile(string uriString, string destination)
		{
			var uri= new Uri(uriString);
			try
			{
				var client = CreateClient(uri);
				client.DownloadFile(uri, destination);
			}
			catch (WebException) 
			{
				if (UriIsCampfire(uri) && uri.Scheme == "https")
				{
					var newUri = new UriBuilder(uri) {Scheme = "http", Port = 80};
					DownloadFile(newUri.ToString(), destination);
				}
				else
				{
					throw;
				}
			}
			return Unit.Default;
		}

		public Upload UploadFile(int roomId, UploadFileParams uploadFileParams, IObserver<ProgressState> progressObserver)
		{
			string relativeUri = String.Format("room/{0}/uploads.xml", roomId);
			var uri = FormatUri(relativeUri);
			var request = CreateRequest(uri);
			if (UriIsCampfire(uri))
			{
				request.Headers["Authorization"] = Convert.ToBase64String(
					Encoding.UTF8.GetBytes(_loginInfo.Token + ":X"));
			}
			request.AllowWriteStreamBuffering = false;
			var builder = new MultipartFormDataBuilder();
			request.ContentType = "multipart/form-data; boundary=" + builder.Boundary;

			builder.AddStream(uploadFileParams.Stream, "upload", uploadFileParams.Filename, uploadFileParams.ContentType);

			request.ContentLength = builder.ContentLength;

			using(var requestStream = request.GetRequestStream())
			{
				builder.Write(requestStream, progressObserver);
				requestStream.Flush();
			}

			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.Created)
			{
					throw new Exception("Unexpected response code: " + response.StatusCode);
			}
			using (var responseStream = response.GetResponseStream())
			{
				var deserializer = GetSerializer<Upload>(null);
				Debug.Assert(responseStream != null, "responseStream != null");
				return (Upload) deserializer.Deserialize(responseStream);
			}
		}

		public Message[] GetMessages(int id, int? sinceId = null)
		{
			var uri = String.Format("room/{0}/recent.xml", id);

			if (sinceId != null)
			{
				uri += "?since_message_id=" + sinceId;
			}

			return Get<Message[]>(uri, "messages");
		}

		public bool CheckAccountExists(string account)
		{
			var uri = new Uri(String.Format(CampfireBaseUri, account));
			var request = CreateRequest(uri);
			request.Method = "HEAD";

			try
			{
				var response = (HttpWebResponse)request.GetResponse();
				return response.StatusCode != HttpStatusCode.NotFound;
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
				{
					throw;
				}
				if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
				{
					return true;
				}
				throw;
			}
		}

		public HeadInfo Head(string url)
		{
			var uri = new Uri(url);
			var request = CreateRequest(uri);

			request.Method = "HEAD";

			try
			{
				var response = ((HttpWebResponse) request.GetResponse());

				return new HeadInfo(url, response.ContentType, true);

			}
			catch (Exception)
			{
				return new HeadInfo(url, null, false);
			}
			
		}

		public string AccountName
		{
			get { return _loginInfo != null ? _loginInfo.Account : string.Empty; }
		}

		public User GetMe()
		{
			var uri = String.Format("users/me.xml");
			return Get<User>(uri);
		}

		public ConnectivityState CheckConnectivity()
		{
			Func<bool, bool> check = useProxy =>
				{
					var rq = WebRequest.Create("http://campfirenow.com");
					rq.Method = "HEAD";
					rq.Timeout = (int)TimeSpan.FromSeconds(2).TotalMilliseconds;

					if (!useProxy)
					{
						rq.Proxy = null;
					}

					try
					{
						using(rq.GetResponse())
						{
							return true;
						}
					}
					catch (Exception)
					{
						return false;
					}

				};


			var withProxy = check(true);
			var withoutProxy = check(false);
			return new ConnectivityState(withProxy, withoutProxy);
		}

		private T Post<T>(string relativeUri, object data = null, HttpStatusCode expectedCode = HttpStatusCode.OK,
			string returnedRoot = null)
			where T: class
		{
			try
			{
				return DoPost<T>(relativeUri, data, expectedCode, returnedRoot);
			}
			catch (WebException wex)
			{
				if (wex.Status == WebExceptionStatus.Timeout)
				{
					throw new TimeoutException("Campfire API call timed out", wex);
				}
				throw;
			}
		}

		private T DoPost<T>(string relativeUri, object data, HttpStatusCode expectedCode, string returnedRoot) where T : class
		{
			var uri = FormatUri(relativeUri);
			var request = CreateRequest(uri);

			//if (_cookie != null)
			//{
			//    request.Headers.Add("Cookie", _cookie);
			//}

			if (data != null)
			{
				var serializer = new XmlSerializer(data.GetType());
				using (var stream = request.GetRequestStream())
				{
					serializer.Serialize(stream, data);
				}
			}

			using (var response = (HttpWebResponse)request.GetResponse())
			{
				if (response.StatusCode != expectedCode)
				{
					throw new Exception("Unexpected response code: " + response.StatusCode);
				}

				var responseStream = response.GetResponseStream();
				if (typeof (T) == typeof (NoResponse) || responseStream == null)
				{
					return null;
				}

				var deserializer = GetSerializer<T>(returnedRoot);
				return (T) deserializer.Deserialize(responseStream);
			}
		}

		private XmlSerializer GetSerializer<T>(string root)
		{
			return _xmlSerializers[new SerializerEntry(root, typeof (T))];
		}

		private HttpWebRequest CreateRequest(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Credentials = CreateCredentials(uri);
			request.Method = "POST";
			if (!_settings.Network.UseProxy)
			{
				request.Proxy = null;
			}
			//request.Proxy = new WebProxy("http://127.0.0.1:8888");

			request.Timeout = _defaultTimeout;
			request.KeepAlive = false;

			request.ContentType = "application/xml";

			request.UserAgent = UserAgent;

			if (Cookie != null && UriIsCampfire(uri))
			{
				request.Headers.Add("Cookie", Cookie);
			}

			return request;
		}

		private T Get<T>(string relativeUri, string root = null)
		{
			try
			{
				return DoGet<T>(relativeUri, root);
			}
			catch (WebException wex)
			{
				if (wex.Status == WebExceptionStatus.Timeout)
				{
					throw new TimeoutException("Campfire API call has timed out", wex);
				}
				throw;
			}
		}

		private T DoGet<T>(string relativeUri, string root)
		{
			var uri = FormatUri(relativeUri);
			using (var client = CreateClient(uri))
			{

				var xml = client.DownloadString(uri);

				if (UriIsCampfire(uri))
				{
					_cookie = client.ResponseHeaders["set-cookie"];
				}

				var serializer = GetSerializer<T>(root);

				return (T) serializer.Deserialize(new StringReader(xml));
			}
		}

		private Uri FormatUri(string relativeUri)
		{
			var baseUri = new Uri(String.Format(CampfireBaseUri, _loginInfo.Account));
			var uri = new Uri(baseUri, relativeUri);
			return uri;
		}

		private WebClient CreateClient(Uri uri)
		{
			var client = new WebClientWithTimeout {Credentials = CreateCredentials(uri), Timeout = _defaultTimeout};
			client.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
			if (!_settings.Network.UseProxy)
			{
				client.Proxy = null;
			}
			//client.Proxy = new WebProxy("http://127.0.0.1:8888");
			if (Cookie != null && UriIsCampfire(uri))
			{
				client.Headers.Add("Cookie", Cookie);
			}
			return client;
		}

		private NetworkCredential CreateCredentials(Uri uri)
		{
			if (_loginInfo != null && UriIsCampfire(uri))
			{
				return new NetworkCredential(_loginInfo.Token, "X");
			}
			return null;
		}

		private bool UriIsCampfire(Uri uri)
		{
			if (_loginInfo == null)
			{
				return false;
			}
			Uri campfireUri;
			if (!Uri.TryCreate(String.Format(CampfireBaseUri, _loginInfo.Account), UriKind.Absolute, 
				out campfireUri))
			{
				return false;
			}
			return String.Equals(uri.Host, campfireUri.Host, StringComparison.OrdinalIgnoreCase);
		}

		private class NoResponse{}

		private class SerializerEntry
		{
			private readonly string _rootElement;
			private readonly Type _type;

			public SerializerEntry(string rootElement, Type type)
			{
				_rootElement = rootElement;
				_type = type;
			}

			public SerializerEntry(Type type)
			{
				_type = type;
			}


			private bool Equals(SerializerEntry other)
			{
				if (ReferenceEquals(null, other)) return false;
				if (ReferenceEquals(this, other)) return true;
				return Equals(other._rootElement, _rootElement) && Equals(other._type, _type);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != typeof (SerializerEntry)) return false;
				return Equals((SerializerEntry) obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return ((_rootElement != null ? _rootElement.GetHashCode() : 0)*397) ^ (_type != null ? _type.GetHashCode() : 0);
				}
			}
		}

		public IDisposable Stream(int id, Action<Message> action, IObserver<ConnectionState> observer)
		{
			return CreateStreamingObservable(id, observer).Subscribe(action, e => 
				observer.OnNext(new ConnectionState(id, false, e)));
		}

		private IObservable<Message> CreateStreamingObservable(int id, IObserver<ConnectionState> observer)
		{
			return Observable.Create<Message>(o =>
				{
					var cts = new CancellationTokenSource();

					Observable.Start(() =>
						{
							try
							{
								observer.OnNext(new ConnectionState(id, true));
								RunStream(id, o, cts.Token);
							}
							catch (Exception ex)
							{
								o.OnError(ex);
							}
						});
					return () => cts.Cancel();
				})
				.Catch<Message, StreamingDisconnectedException>(ex => RestartConnection(ex, id, 2, observer))
				.Catch<Message, WebException>(ex => RestartConnection(ex, id, 30, observer))
				.Catch<Message, IOException>(ex => RestartConnection(ex, id, 30, observer));
		}

		private IObservable<Message> RestartConnection(Exception ex, int roomId, int delay, IObserver<ConnectionState> observer)
		{
			return Observable.Return(0).Do(_ => observer.OnNext(new ConnectionState(roomId, false, ex)))
				.Delay(TimeSpan.FromSeconds(delay))
				.SelectMany(_ => CreateStreamingObservable(roomId, observer));
		}

		private void RunStream(int id, IObserver<Message> observer, CancellationToken ct)
		{
			var uri = String.Format("http://streaming.campfirenow.com/room/{0}/live.json", id);
			var request = CreateRequest(new Uri(uri));

			var servicePoint = ServicePointManager.FindServicePoint(new Uri(uri));
			servicePoint.SetTcpKeepAlive(true, 2000, 500);

			request.Method = "GET";

			request.Timeout = -1;

			string credentials = String.Format("{0}:{1}", _loginInfo.Token, "X");
			byte[] bytes = Encoding.ASCII.GetBytes(credentials);
			string base64 = Convert.ToBase64String(bytes);
			string authorization = String.Concat("basic ", base64);
			request.Headers.Add("Authorization", authorization);

			Debug.WriteLine(String.Format("Streaming room id {0}. Received response", id));

			using (WebResponse response = request.GetResponse())
			{
				try
				{
					var stream = response.GetResponseStream();
					if (stream == null)
					{
						throw new StreamingDisconnectedException();
					}
					var streamReader = new StreamReader(stream);
					while (true)
					{
						string line = streamReader.ReadLine();
						if (ct.IsCancellationRequested)
						{
							break;
						}

						Debug.WriteLine(String.Format("Streaming room id {0}. Received line: {1}", id, (line ?? "<null>")));
						if (line == null)
						{
							throw new StreamingDisconnectedException();
						}
						if (line.Trim() == String.Empty)
						{
							continue;
						}

						JsonReader reader = new JsonTextReader(new StringReader(line));

						var serializer = new JsonSerializer();
						var message = serializer.Deserialize<Message>(reader);

						observer.OnNext(message);
					}
				}
				finally
				{
					request.Abort();
				}
			}
		}
	}


}
