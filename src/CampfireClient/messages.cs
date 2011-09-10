using Rogue.MetroFire.CampfireClient.Serialization;

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
}
