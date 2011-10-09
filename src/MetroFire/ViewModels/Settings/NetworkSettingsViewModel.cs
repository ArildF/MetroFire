using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class NetworkSettingsViewModel : ReactiveObject, ISettingsSubPage
	{
		private readonly NetworkSettings _settings;
		private bool _useProxy;

		public NetworkSettingsViewModel(NetworkSettings settings)
		{
			_settings = settings;
		}

		public bool UseProxy
		{
			get { return _useProxy; }
			set { this.RaiseAndSetIfChanged(vm => vm.UseProxy, ref _useProxy, value); }
		}

		public string Title
		{
			get { return "Network"; }
		}

		public void Commit()
		{
			_settings.UseProxy = UseProxy;
		}
	}
}
