using System;
using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public class RequestLoginMessage
	{
		public LoginInfo LoginInfo { get; private set; }

		public RequestLoginMessage(LoginInfo loginInfo)
		{
			LoginInfo = loginInfo;
		}
	}

	public class LoginSuccessfulMessage
	{}

	public class AccountUpdated
	{
		public IAccount Account { get; private set; }

		public AccountUpdated(IAccount account)
		{
			Account = account;
		}
	}

	public class RoomListMessage
	{
		public Room[] Rooms { get; private set; }

		public RoomListMessage(Room[] rooms)
		{
			Rooms = rooms;
		}
	}

	public class RequestRoomListMessage
	{
	}

	public class RequestRoomPresenceMessage{}

	public class RoomPresenceMessage
	{
		private readonly Room[] _rooms;

		public RoomPresenceMessage(Room[] rooms)
		{
			_rooms = rooms;
		}

		public Room[] Rooms
		{
			get { return _rooms; }
		}

		public bool IsPresentIn(int id)
		{
			return _rooms.Any(r => r.Id == id);
		}

		public bool IsPresentIn(string caption)
		{
			return _rooms.Any(r => r.Name.Equals(caption, StringComparison.InvariantCultureIgnoreCase));
		}
	}

	public class UserJoinedRoomMessage
	{
		public int Id { get; private set; }

		public UserJoinedRoomMessage(int id)
		{
			Id = id;
		}
	}

	public class RequestJoinRoomMessage
	{
		public int Id { get; private set; }

		public RequestJoinRoomMessage(int id)
		{
			Id = id;
		}
	}

	public class RequestSpeakInRoomMessage
	{
		public int Id { get; private set; }

		public string Message { get; private set; }

		public RequestSpeakInRoomMessage(int id, string message)
		{
			Id = id;
			Message = message;
		}
	}

	public class RequestRecentMessagesMessage
	{
		public int RoomId { get; private set; }
		public int? SinceMessageId { get; private set; }

		public RequestRecentMessagesMessage(int roomId, int? sinceMessageId = null)
		{
			RoomId = roomId;
			SinceMessageId = sinceMessageId;
		}
	}


	public class MessagesReceivedMessage
	{
		public int RoomId { get; private set; }
		public Message[] Messages { get; private set; }

		public int? SinceMessageId { get; private set; }

		public MessagesReceivedMessage(int roomId, Message[] messages, int? sinceMessageId)
		{
			RoomId = roomId;
			Messages = messages;
			SinceMessageId = sinceMessageId;
		}
	}

	public class RequestRoomInfoMessage
	{
		public int Id { get; private set; }

		public RequestRoomInfoMessage(int id)
		{
			Id = id;
		}
	}

	public class RoomInfoReceivedMessage
	{
		public Room Room { get; private set; }

		public RoomInfoReceivedMessage(Room room)
		{
			Room = room;
		}
	}

	public class RequestUserInfoMessage
	{
		public IEnumerable<int> UserIds { get; private set; }

		public RequestUserInfoMessage(IEnumerable<int> ids)
		{
			UserIds = ids;
		}

		public RequestUserInfoMessage(params int[] ids)
		{
			UserIds = ids;
		}
	}


	public class UserInfoReceivedMessage
	{
		public User User { get; private set; }

		public UserInfoReceivedMessage(User user)
		{
			User = user;
		}
	}

	public class ExceptionMessage
	{
		public Exception Exception { get; set; }

		public ExceptionMessage(Exception exception)
		{
			Exception = exception;
		}
	}

	public enum LogMessageType
	{
		Info,
		Warning,
		Debug,
		Error
	}

	public class LogMessage
	{
		public LogMessageType LogMessageType { get; private set; }
		public string Text { get; private set; }

		public LogMessage(LogMessageType logMessageType, string text, params object[] args)
		{
			LogMessageType = logMessageType;
			Text = String.Format(text, args);
		}
	}

}
