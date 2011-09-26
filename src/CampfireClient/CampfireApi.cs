﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Rogue.MetroFire.CampfireClient.Serialization;
 
namespace Rogue.MetroFire.CampfireClient
{

	public class CampfireApi : ICampfireApi
	{
		private LoginInfo _loginInfo;

		public Account GetAccountInfo()
		{
			return Get<Account>("/account.xml");
		}

		public Room[] ListRooms()
		{
			var roomArray = Get<Room[]>("rooms.xml", "rooms");
			return roomArray;
		}

		public void Join(int id)
		{
			string relativeUri = String.Format("room/{0}/join.xml", id);
			Post(relativeUri);
		}

		public void Speak(int id, string text)
		{
			string relativeUri = String.Format("room/{0}/speak.xml", id);
			var msg = new Message {Type = "TextMessage", Body = text};

			Post(relativeUri, msg, HttpStatusCode.Created);
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
			Post(relativeUri);
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

		public Message[] GetMessages(int id)
		{
			var uri = String.Format("room/{0}/recent.xml", id);

			return Get<Message[]>(uri, "messages");
		}

		private void Post(string relativeUri, object data = null, HttpStatusCode expectedCode = HttpStatusCode.OK)
		{
			var uri = FormatUri(relativeUri);
			var request = CreateRequest(uri);

			if (data != null)
			{
				var serializer = new XmlSerializer(data.GetType());
				using (var stream = request.GetRequestStream())
				{
					serializer.Serialize(stream, data);
					
				}
			}

			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != expectedCode)
			{
				throw new Exception("Unexpected response code: " + response.StatusCode);
			}

		}

		private HttpWebRequest CreateRequest(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Credentials = CreateCredentials();
			request.Method = "POST";

			request.ContentType = "application/xml";

			return request;
		}

		private T Get<T>(string relativeUri, string root = null)
		{
			var client = CreateClient();
			
			var uri = FormatUri(relativeUri);

			var xml = client.DownloadString(uri);

			var serializer = root != null
			                 	? new XmlSerializer(typeof (T), new XmlRootAttribute(root))
			                 	: new XmlSerializer(typeof (T));

			return (T) serializer.Deserialize(new StringReader(xml));
		}

		private Uri FormatUri(string relativeUri)
		{
			var baseUri = new Uri(String.Format("https://{0}.campfirenow.com", _loginInfo.Account));
			var uri = new Uri(baseUri, relativeUri);
			return uri;
		}

		private WebClient CreateClient()
		{
			var client = new WebClient {Credentials = CreateCredentials()};
			return client;
		}

		private NetworkCredential CreateCredentials()
		{
			return new NetworkCredential(_loginInfo.Token, "X");
		}

		public IDisposable Stream(int id, Action<Message> action, Action<Exception> onError = null)
		{
			return CreateStreamingObservable(id).Subscribe(action, e =>
				{
					if (onError != null)
					{
						onError(e);
					}
				});
		}

		private IObservable<Message> CreateStreamingObservable(int id)
		{
			return RunStream(id).ToObservable()
				.SubscribeOn(Scheduler.ThreadPool)
				.ObserveOn(Scheduler.ThreadPool);
		}

		private IEnumerable<Message> RunStream(int id)
		{
			int secondsToWaitForRetry = 2;
			while(true)
			{
				var uri = String.Format("https://streaming.campfirenow.com/room/{0}/live.json", id);
				var request = CreateRequest(new Uri(uri));
				request.Method = "GET";
				//request.Proxy = new WebProxy("127.0.0.1:8888");

				request.Timeout = -1;

				string credentials = String.Format("{0}:{1}", _loginInfo.Token, "X");
				byte[] bytes = Encoding.ASCII.GetBytes(credentials);
				string base64 = Convert.ToBase64String(bytes);
				string authorization = String.Concat("basic ", base64);
				request.Headers.Add("Authorization", authorization);

				WebResponse response = null;
				try
				{
					response = request.GetResponse();
				}
				catch (WebException e)
				{
					if (e.Status == WebExceptionStatus.Timeout)
					{
						Thread.Sleep(TimeSpan.FromSeconds(secondsToWaitForRetry));
						secondsToWaitForRetry *= 2;
						continue;
					}
					throw;
				}
				secondsToWaitForRetry = 2;
				using (var stream = response.GetResponseStream())
				{
					var streamReader = new StreamReader(stream);
					var str = streamReader.ReadLine();
					if (str == null || str.Trim() == String.Empty)
					{
						continue;
					}

					JsonReader reader = new JsonTextReader(new StringReader(str));

					var serializer = new JsonSerializer();
					var message = serializer.Deserialize<Message>(reader);

					yield return message;
				}
			}

		}
	}


}
