using System;
using System.Xml.Serialization;

namespace Rogue.MetroFire.CampfireClient.Serialization
{
	public interface IAccount
	{
		string Name { get; }
		string Subdomain { get; }
		string Plan { get; }
		string Storage { get; }
	}

	[XmlRoot("account")]
	public class Account : IAccount
	{
		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }
		
		[XmlElement("subdomain")]
		public string Subdomain { get; set; }

		[XmlElement("plan")]
		public string Plan { get; set; }

		[XmlElement("storage")]
		public string Storage { get; set; }
		
		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[XmlElement("updated-at")]
		public DateTime UpdatedAt { get; set; }
	}

	[XmlRoot("rooms")]
	public class RoomArray
	{
		[XmlElement("room")]
		public Room[] Rooms { get; set; }
	}


	public interface IRoom
	{
		[XmlElement("name")]
		string Name { get; }

		[XmlElement("topic")]
		string Topic { get; }
	}

	[XmlRoot("room")]
	public class Room : IRoom
	{
		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("topic")]
		public string Topic { get; set; }

		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[XmlElement("updated-at")]
		public DateTime UpdatedAt { get; set; }
	}
}
