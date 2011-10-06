﻿using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class GeneralSettingsViewModel : ReactiveObject, ISettingsSubPage
	{
		private readonly GeneralSettings _settings;
		private bool _useStandardWindowsChrome;

		public GeneralSettingsViewModel(GeneralSettings settings)
		{
			_settings = settings;
		}

		public string Title
		{
			get { return "General"; }
		}


		public bool UseStandardWindowsChrome
		{
			get { return _useStandardWindowsChrome; }
			set { this.RaiseAndSetIfChanged(vm => vm.UseStandardWindowsChrome, ref _useStandardWindowsChrome, value); }
		}

		public void Save()
		{
			_settings.UseStandardWindowsChrome = UseStandardWindowsChrome;

		}
	}
}
