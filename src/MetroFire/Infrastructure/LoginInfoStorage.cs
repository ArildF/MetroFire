using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class LoginInfoStorage : ILoginInfoStorage
	{
		public LoginInfo GetStoredLoginInfo()
		{
			string path = GetPath();
			if (!File.Exists(path))
			{
				return null;
			}
			using (var stream = File.OpenRead(path))
			{
				var formatter = new BinaryFormatter();
				return (LoginInfo) formatter.Deserialize(stream);
			}

		}

		private string GetPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MetroFire");
		}

		public void PersistLoginInfo(LoginInfo info)
		{
			string path = GetPath();
			using (var stream = File.OpenWrite(path))
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, info);
			}

		}
	}
}
