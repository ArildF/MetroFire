using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class GeneralSettingsViewModel : ReactiveObject, ISettingsSubPage
	{
		private readonly GeneralSettings _settings;
		private bool _useStandardWindowsChrome;
		private int _maxBackLog;
		private int _showAnimatedGifsForSeconds;

		public GeneralSettingsViewModel(GeneralSettings settings)
		{
			_settings = settings;
			UseStandardWindowsChrome = _settings.UseStandardWindowsChrome;
			MaxBacklog = _settings.MaxBackLog;
			ShowAnimatedGifsForSeconds = _settings.ShowAnimatedGifsForSeconds;
		}

		public string Title
		{
			get { return "_General"; }
		}


		public bool UseStandardWindowsChrome
		{
			get { return _useStandardWindowsChrome; }
			set { this.RaiseAndSetIfChanged(vm => vm.UseStandardWindowsChrome, ref _useStandardWindowsChrome, value); }
		}

		public int MaxBacklog
		{
			get { return _maxBackLog; }
			set { this.RaiseAndSetIfChanged(vm => vm.MaxBacklog, ref _maxBackLog, value); }
		}

		public int ShowAnimatedGifsForSeconds
		{
			get { return _showAnimatedGifsForSeconds; }
			set { this.RaiseAndSetIfChanged(vm => vm.ShowAnimatedGifsForSeconds, ref _showAnimatedGifsForSeconds, value); }
		}

		public void Commit()
		{
			_settings.UseStandardWindowsChrome = UseStandardWindowsChrome;
			_settings.MaxBackLog = MaxBacklog;
			_settings.ShowAnimatedGifsForSeconds = ShowAnimatedGifsForSeconds;

		}
	}
}
