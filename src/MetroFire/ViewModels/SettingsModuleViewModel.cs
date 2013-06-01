using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels.Settings;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class SettingsModuleViewModel : ReactiveObject, ISettingsViewModel, IMainModule
	{
		private ISettingsSubPage _selectedPage;

		public SettingsModuleViewModel(IMessageBus bus, ISettingsLoader loader, ISettingsSaver saver)
		{
			SettingsCommand = new ReactiveCommand();
			bus.RegisterMessageSource(SettingsCommand.Select(_ => new NavigateMainModuleMessage(ModuleNames.SettingsModule)));
			BackCommand = new ReactiveCommand();
			bus.RegisterMessageSource(BackCommand.Select(_ => new NavigateBackMainModuleMessage()));

			var settings = loader.Load();

			SettingsViewModels = new ISettingsSubPage[] {
				new GeneralSettingsViewModel(settings.General), 
				new NetworkSettingsViewModel(settings.Network),
				new NotificationSettingsViewModel(settings.Notification),
				new AboutViewModel(),

			};

			SelectedPage = SettingsViewModels.First();

			bus.RegisterMessageSource(bus.Listen<NavigateSettingsPageMessage>().Do(
					msg => SelectedPage = SettingsViewModels.First(vm => vm.GetType().Name.StartsWith(msg.SettingsPage)))
				.Select(_ => new NavigateMainModuleMessage(ModuleNames.SettingsModule)));

			SaveCommand = new ReactiveCommand();
			bus.RegisterMessageSource(
				SaveCommand.Do(_ => CommitViewModels()).Do(_ => saver.Save(settings)).Select(_ => new NavigateBackMainModuleMessage()));
			NavigationContent = null;
		}

		private void CommitViewModels()
		{
			foreach (var settingsViewModel in SettingsViewModels)
			{
				settingsViewModel.Commit();
			}
		}

		public ISettingsSubPage SelectedPage
		{
			get { return _selectedPage; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedPage, ref _selectedPage, value); }
		}

		public ReactiveCommand BackCommand { get; private set; }

		public ISettingsSubPage[] SettingsViewModels { get; private set; }

		public ReactiveCommand SettingsCommand { get; private set; }

		public ReactiveCommand SaveCommand { get; private set; }
		ICommand ISettingsViewModel.SettingsCommand { get { return SettingsCommand; } }
		public object NavigationContent { get; private set; }
	}
}
