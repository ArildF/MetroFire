using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
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
	}


}
