using System;
using System.Xml.Serialization;

namespace Rogue.MetroFire.CampfireClient.Serialization
{
	public class Account
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
}
