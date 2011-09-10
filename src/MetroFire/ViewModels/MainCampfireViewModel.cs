using System;
using System.Linq;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class MainCampfireViewModel : ReactiveObject, IMainCampfireViewModel
	{
		private readonly ILobbyModule _lobbyModule;
		private readonly IMessageBus _bus;
		private readonly IRoomModuleCreator _creator;
		private IModule _activeModule;
		private ReactiveCollection<ModuleViewModel> _currentModules;

		public MainCampfireViewModel(ILobbyModule lobbyModule, IMessageBus bus, IRoomModuleCreator creator)
		{
			_lobbyModule = lobbyModule;
			_bus = bus;
			_creator = creator;

			Modules = new ReactiveCollection<IModule>{_lobbyModule};

			
			_bus.Listen<ActivateModuleMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg =>
					{
						CurrentModules = Modules.CreateDerivedCollection(module => new ModuleViewModel(module));
					});
			
			bus.RegisterMessageSource(bus.Listen<ModuleLoaded>().Where(msg => msg.ModuleName == ModuleNames.MainCampfireView)
				.Select(_ => new ActivateModuleMessage(ModuleNames.MainCampfireView, _lobbyModule)));
			_activeModule = _lobbyModule;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(SyncModuleList);

			_bus.Listen<ActivateModuleByIdMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(HandleActivateModuleById);

			ActivateModuleCommand = new ReactiveCommand();
			ActivateModuleCommand.OfType<ModuleViewModel>().Subscribe(HandleActivateModule);

		}

		private void HandleActivateModuleById(ActivateModuleByIdMessage msg)
		{
			ActiveModule = CurrentModules.Select(m => m.Module).Where(m => m.Id == msg.Id).FirstOrDefault();
		}

		private IModule ActiveModule
		{
			get { return _activeModule; }
			set
			{
				if (_activeModule == value)
				{
					return;
				}

				_activeModule = value;
				_bus.SendMessage(new ActivateModuleMessage(ModuleNames.MainCampfireView, _activeModule));
			}
		}

		public ReactiveCommand ActivateModuleCommand { get; set; }


		private void HandleActivateModule(ModuleViewModel viewModel)
		{
			ActiveModule = viewModel.Module;
		}

		private void SyncModuleList(RoomPresenceMessage roomPresenceMessage)
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


		public ReactiveCollection<ModuleViewModel> CurrentModules
		{
			get { return _currentModules; }
			set { this.RaiseAndSetIfChanged(vm => vm.CurrentModules, ref _currentModules, value); }
		}

		private ReactiveCollection<IModule> Modules { get;  set; }
	}
}
