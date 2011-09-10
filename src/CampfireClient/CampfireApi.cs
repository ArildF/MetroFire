using System;
using System.IO;
using System.Net;
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

		public void SetLoginInfo(LoginInfo loginInfo)
		{
			_loginInfo = loginInfo;
		}

		public Room[] ListPresence()
		{
			var roomArray = Get<Room[]>("presence.xml", "rooms");
			return roomArray;
		}

		private void Post(string relativeUri)
		{
			var uri = FormatUri(relativeUri);
			var request = CreateRequest(uri);

			var response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception("Unexpected response code: " + response.StatusCode);
			}

		}

		private HttpWebRequest CreateRequest(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Credentials = CreateCredentials();
			request.Method = "POST";

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
			var client = new WebClient {Credentials = CreateCredentials(), Proxy = null};
			return client;
		}

		private NetworkCredential CreateCredentials()
		{
			return new NetworkCredential(_loginInfo.Token, "X");
		}
	}
}
