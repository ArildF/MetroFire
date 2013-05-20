namespace Rogue.MetroFire.UI.Settings
{
	public class GeneralSettings
	{
		public GeneralSettings()
		{
			MaxBackLog = 500;
			ShowAnimatedGifsForSeconds = 60;
		}

		public bool UseStandardWindowsChrome { get; set; }

		public int MaxBackLog { get; set; }

		public int ShowAnimatedGifsForSeconds { get; set; }
	}
}