using System;

namespace Rogue.MetroFire.CampfireClient
{
	[Serializable]
	public class LoginInfo
	{
		public string Account { get; private set; }
		public string Token { get; private set; }

		public LoginInfo(string account, string token)
		{
			Account = account;
			Token = token;
		}
	}
}
