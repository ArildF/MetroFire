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
}
