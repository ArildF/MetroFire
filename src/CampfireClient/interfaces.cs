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
		void Join(int id);
		void Speak(int id, string text);
		Message[] GetMessages(int id);
		Room GetRoom(int id);
		User GetUser(int id);
	}


}
