using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class GeneralSettingsViewModel : ReactiveObject, ISettingsSubPage
	{
		private readonly GeneralSettings _settings;

		public GeneralSettingsViewModel(GeneralSettings settings)
		{
			_settings = settings;
		}

		public string Title
		{
			get { return "General"; }
		}

		public void Save()
		{
			
		}
	}
}
