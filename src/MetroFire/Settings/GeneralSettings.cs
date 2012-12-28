namespace Rogue.MetroFire.UI.Settings
{
	public class GeneralSettings
	{
		public GeneralSettings()
		{
			MaxBackLog = 500;
		}

		public bool UseStandardWindowsChrome { get; set; }

		public int MaxBackLog { get; set; }
	}
}