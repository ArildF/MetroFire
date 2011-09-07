using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class MainCampfireViewModel : ReactiveObject, IMainCampfireViewModel
	{
		private readonly ILobbyModule _lobbyModule;

		public MainCampfireViewModel(ILobbyModule lobbyModule, IMessageBus bus)
		{
			_lobbyModule = lobbyModule;

			Modules = new ReactiveCollection<string> {_lobbyModule.Caption};


			bus.RegisterMessageSource(bus.Listen<ModuleLoaded>().Where(msg => msg.ModuleName == ModuleNames.MainCampfireView)
				.Select(_ => new ActivateModuleMessage(ModuleNames.MainCampfireView, _lobbyModule)));
		}

		public ReactiveCollection<string> Modules { get; private set; }
	}

}
