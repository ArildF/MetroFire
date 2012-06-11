using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class SettingsViewModel : ReactiveObject, ISettingsViewModel
	{
		public SettingsViewModel(IMessageBus bus, ISettingsLoader loader, ISettingsSaver saver)
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

			SaveCommand = new ReactiveCommand();
			bus.RegisterMessageSource(
				SaveCommand.Do(_ => CommitViewModels()).Do(_ => saver.Save(settings)).Select(_ => new NavigateBackMainModuleMessage()));
		}

		private void CommitViewModels()
		{
			foreach (var settingsViewModel in SettingsViewModels)
			{
				settingsViewModel.Commit();
			}
		}

		public ReactiveCommand BackCommand { get; private set; }

		public ISettingsSubPage[] SettingsViewModels { get; private set; }

		public ReactiveCommand SettingsCommand { get; private set; }

		public ReactiveCommand SaveCommand { get; private set; }
	}
}
