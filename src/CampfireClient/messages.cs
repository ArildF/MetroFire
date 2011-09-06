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
}
