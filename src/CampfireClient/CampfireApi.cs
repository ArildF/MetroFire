using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public class CampfireApi
	{
		private readonly string _token;
		private readonly Uri _baseUri;

		public CampfireApi(string url, string token)
		{
			_baseUri = new Uri(url);
			_token = token;
		}

		public Account GetAccountInfo()
		{
			return Get<Account>("/account.xml");
		}

		public Room[] ListRooms()
		{
			var roomArray = Get<RoomArray>("rooms.xml");
			return roomArray.Rooms;
		}

		private T Get<T>(string relativeUri)
		{
			var client = new WebClient {Credentials = new NetworkCredential(_token, "X"), Proxy = null};

			var uri = new Uri(_baseUri, relativeUri);

			var xml = client.DownloadString(uri);

			return (T) new XmlSerializer(typeof (T)).Deserialize(new StringReader(xml));
		}
	}
}
