using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public interface ICampfire
	{
		
	}

	public interface ICampfireApi
	{
		Account GetAccountInfo();
		Room[] ListRooms();
		void SetLoginInfo(LoginInfo loginInfo);
	}


}
