using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class SettingsViewModel : ReactiveObject, ISettingsViewModel
	{
		public SettingsViewModel(IMessageBus bus)
		{
			SettingsCommand = new ReactiveCommand();
			bus.RegisterMessageSource(SettingsCommand.Select(_ => new NavigateMainModuleMessage(ModuleNames.SettingsModule)));
			BackCommand = new ReactiveCommand();
			bus.RegisterMessageSource(BackCommand.Select(_ => new NavigateBackMainModuleMessage()));

			SettingsViewModels = new ISettingsSubPage[] {new GeneralSettingsViewModel(), new NetworkSettingsViewModel()};
		}

		public ReactiveCommand BackCommand { get; private set; }

		public ISettingsSubPage[] SettingsViewModels { get; private set; }

		public ReactiveCommand SettingsCommand { get; private set; }
	}

}
