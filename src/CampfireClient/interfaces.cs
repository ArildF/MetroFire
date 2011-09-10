using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public interface ICampfire
	{
		IAccount Account { get; }
		IEnumerable<Room> Rooms { get; }
	}

	public interface ICampfireApi
	{
		Account GetAccountInfo();
		Room[] ListRooms();
		void SetLoginInfo(LoginInfo loginInfo);
		Room[] ListPresence();
	}


}
