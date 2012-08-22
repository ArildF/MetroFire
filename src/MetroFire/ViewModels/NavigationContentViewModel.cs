using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class NavigationContentViewModel : ReactiveObject, INavigationContentViewModel
	{
		private readonly ISettingsViewModel _settingsViewModel;
		private bool _showUpdate;
		private bool _showProgress;
		private int _progressValue;

		public NavigationContentViewModel(ISettingsViewModel settingsViewModel, IMessageBus bus)
		{
			_settingsViewModel = settingsViewModel;
			bus.Listen<AppUpdateAvailableMessage>().SubscribeOnceUI(_ =>
				{
					ShowUpdate = true;
				});
			bus.Listen<AppUpdateProgressMessage>().SubscribeUI(msg =>
				{
					ShowProgress = true;
					ShowUpdate = false;
					ProgressValue = msg.ProgressPercentage;
				});

			StartUpdateCommand = new ReactiveCommand();
			bus.RegisterMessageSource(StartUpdateCommand.Select(c => new RequestAppUpdateMessage()));
		}

		public ReactiveCommand StartUpdateCommand { get; private set; }

		public ICommand SettingsCommand
		{
			get { return _settingsViewModel.SettingsCommand; }
		}

		public bool ShowUpdate
		{
			get { return _showUpdate; }
			set{ this.RaiseAndSetIfChanged(vm => vm.ShowUpdate, ref _showUpdate, value);}
		}

		public bool ShowProgress
		{
			get { return _showProgress; }
			set { this.RaiseAndSetIfChanged(vm => vm.ShowProgress, ref _showProgress, value); }
		}
		public int ProgressValue
		{
			get { return _progressValue; }
			set { this.RaiseAndSetIfChanged(vm => vm.ProgressValue, ref _progressValue, value); }
		}
	}
}
