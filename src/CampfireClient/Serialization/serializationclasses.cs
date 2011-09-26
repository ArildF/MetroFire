using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rogue.MetroFire.CampfireClient.Serialization
{
	public interface IAccount
	{
		string Name { get; }
		string Subdomain { get; }
		string Plan { get; }
		string Storage { get; }
	}

	public enum MessageType
	{
		Unknown = 0,
		TextMessage = 1,
		TimestampMessage,
		KickMessage,
		EnterMessage,
		LeaveMessage,
		PasteMessage,
		AdvertisementMessage

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


	public interface IRoom
	{
		int Id { get; }

		string Name { get; }

		string Topic { get; }
		User[] Users { get; }
	}

	[XmlType("room")]
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

		[XmlArray("users")]
		public User[] Users { get; set; }
	}

	[XmlType("user")]
	public class User
	{
		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[XmlElement("type")]
		public string Type { get; set; }

		[XmlElement("avatar-url")]
		public string AvatarUrl { get; set; }

		[XmlElement("admin")]
		public bool Admin { get; set; }
	}

	[XmlType("message")]
	public class Message
	{
		private string _messageTypeString;

		[XmlElement("id")]
		public int Id { get; set; }

		[JsonProperty("created_at")]
		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("room_id")]
		[XmlElement("room-id")]
		public int RoomId { get; set; }

		public int? UserId
		{
			get { return String.IsNullOrEmpty(UserIdString) ? (int?) null : int.Parse(UserIdString); }
		}

		[JsonProperty("user_id")]
		[XmlElement("user-id")]
		public string UserIdString { get; set; }

		[XmlElement("type")]
		public string MessageTypeString
		{
			get { return _messageTypeString; }
			set
			{
				_messageTypeString = value;
				MessageType type;
				if (Enum.TryParse<MessageType>(value, out type))
				{
					Type = type;
				}
			}
		}

		public MessageType Type { get; set; }

		[XmlElement("body")]
		[JsonIgnore]
		public string Body { get; set; }


		[JsonProperty("body")]
		public string JsonBody
		{
			set
			{
				var adjusted = Regex.Replace(value ?? String.Empty, 
					@"[^\r]\n", match => match.Value.Replace("\n", Environment.NewLine));
				Body = adjusted;
			}
		}


	}
}
