using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class GeneralSettingsViewModel : ReactiveObject, ISettingsSubPage
	{
		public string Title
		{
			get { return "General"; }
		}
	}
}
