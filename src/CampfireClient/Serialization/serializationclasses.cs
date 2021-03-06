﻿using System;
using System.Linq;
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
		UploadMessage,
		AdvertisementMessage,
		TopicChangeMessage,
		TweetMessage,
		AllowGuestsMessage,
		DisallowGuestsMessage,
		SoundMessage,
		IdleMessage,
		UnidleMessage,
		LockMessage,
		UnlockMessage,
		ConferenceCreatedMessage,
		ConferenceFinishedMessage,
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
		static User()
		{
			NullUser = new User {Name = ""};
		}
		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("email-address")]
		public string EmailAddress { get; set; }

		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[XmlElement("type")]
		public string Type { get; set; }

		[XmlElement("avatar-url")]
		public string AvatarUrl { get; set; }

		[XmlElement("admin")]
		public bool Admin { get; set; }

		[XmlIgnore]
		public static User NullUser { get; private set; }
	}

	[XmlType("upload")]
	public class Upload
	{
		[XmlElement("byte-size")]
		public int ByteSize { get; set; }

		[XmlElement("content-type")]
		public string ContentType { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("created-at")]
		public DateTime CreatedAt { get; set; }

		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("room-id")]
		public int RoomId { get; set; }

		[XmlElement("user-id")]
		public int UserId { get; set; }

		[XmlElement("full-url")]
		public string FullUrl { get; set; }
	}

	[XmlType("message")]
	public class Message
	{
		private string _messageTypeString;
		private Tweet[] _tweets;

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

		[XmlElement("tweet")]
		public Tweet[] Tweets
		{
			get { return _tweets; }
			set
			{
				_tweets = value;
				if(value != null)
				{
					Tweet = _tweets.LastOrDefault(t => t.Id != null);
				}
			}
		}

		[JsonProperty("tweet")]
		public Tweet Tweet { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as Message;
			return other != null && Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id;
		}

	}

	public class Tweet
	{
		[JsonProperty("author_avatar_url")]
		[XmlElement("author_avatar_url")]
		public string AuthorAvatarUrl { get; set; }

		[JsonProperty("author_username")]
		[XmlElement("author_username")]
		public string AuthorUsername { get; set; }

		[JsonProperty("id")]
		[XmlElement("id")]
		public string Id { get; set; }

		[JsonProperty("message")]
		[XmlElement("message")]
		public string Message { get; set; }
	}
}
