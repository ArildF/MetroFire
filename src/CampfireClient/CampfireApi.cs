using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{

	public class CampfireApi : ICampfireApi
	{
		private LoginInfo _loginInfo;

		public CampfireApi()
		{
		}

		public Account GetAccountInfo()
		{
			return Get<Account>("/account.xml");
		}

		public Room[] ListRooms()
		{
			var roomArray = Get<Room[]>("rooms.xml", "rooms");
			return roomArray;
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

		private T Get<T>(string relativeUri, string root = null)
		{
			var client = new WebClient {Credentials = new NetworkCredential(_loginInfo.Token, "X"), Proxy = null};

			var baseUri = new Uri(String.Format("https://{0}.campfirenow.com", _loginInfo.Account));
			var uri = new Uri(baseUri, relativeUri);

			var xml = client.DownloadString(uri);

			var serializer = root != null
			                 	? new XmlSerializer(typeof (T), new XmlRootAttribute(root))
			                 	: new XmlSerializer(typeof (T));

			return (T) serializer.Deserialize(new StringReader(xml));
		}
	}
}
