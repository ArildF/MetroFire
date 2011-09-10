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
		private Room[] _rooms;

		public RoomPresenceMessage(Room[] rooms)
		{
			_rooms = rooms;
		}

		public bool IsPresentIn(int id)
		{
			return _rooms.Any(r => r.Id == id);
		}
	}
}
