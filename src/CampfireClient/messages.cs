using System;
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


}
