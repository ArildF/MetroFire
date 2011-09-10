using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System.Reactive.Linq;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class MainCampfireViewModel : ReactiveObject, IMainCampfireViewModel
	{
		private readonly ILobbyModule _lobbyModule;
		private readonly IRoomModuleCreator _creator;

		public MainCampfireViewModel(ILobbyModule lobbyModule, IMessageBus bus, IRoomModuleCreator creator)
		{
			_lobbyModule = lobbyModule;
			_creator = creator;

			Modules = new ReactiveCollection<IModule>{_lobbyModule};

			CurrentModuleNames = Modules.CreateDerivedCollection(module => module.Caption);
			
			bus.RegisterMessageSource(bus.Listen<ModuleLoaded>().Where(msg => msg.ModuleName == ModuleNames.MainCampfireView)
				.Select(_ => new ActivateModuleMessage(ModuleNames.MainCampfireView, _lobbyModule)));

			bus.Listen<RoomPresenceMessage>().SubscribeUI(SyncRoomList);
		}

		private void SyncRoomList(RoomPresenceMessage roomPresenceMessage)
		{
			var toRemove = Modules.Where(module => module != _lobbyModule && !roomPresenceMessage.IsPresentIn(module.Caption)).ToList();
			var toAdd = roomPresenceMessage.Rooms.Where(room =>
				!Modules.Any(m => m.Caption.Equals(room.Name, StringComparison.InvariantCultureIgnoreCase))).ToList();

			foreach (var module in toRemove)
			{
				Modules.Remove(module);
			}
			foreach (var room in toAdd)
			{
				var vm = new RoomModuleViewModel(room);

				var newModule = _creator.CreateRoomModule(vm);

				Modules.Add(newModule);
			}
		}

		public ReactiveCollection<string> CurrentModuleNames { get; private set; }

		private ReactiveCollection<IModule> Modules { get;  set; }
	}

}
