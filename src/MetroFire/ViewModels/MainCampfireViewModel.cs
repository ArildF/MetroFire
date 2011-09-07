using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class MainCampfireViewModel : ReactiveObject, IMainCampfireViewModel
	{
		private readonly ILobbyModule _lobbyModule;

		public MainCampfireViewModel(ILobbyModule lobbyModule)
		{
			_lobbyModule = lobbyModule;

			Modules = new ReactiveCollection<string> {_lobbyModule.Caption};
		}

		public ReactiveCollection<string> Modules { get; private set; }
	}

}
