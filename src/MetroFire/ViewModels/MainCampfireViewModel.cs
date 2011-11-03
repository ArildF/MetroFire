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
		private readonly IRoomModuleViewModelFactory _factory;
		private IModule _activeModule;
		private ReactiveCollection<ModuleViewModel> _currentModules;
		private readonly IModule _logModule;

		public MainCampfireViewModel(ILobbyModule lobbyModule, ILogModule logModule, IMessageBus bus, 
			IRoomModuleCreator creator, IRoomModuleViewModelFactory factory, IGlobalCommands globalCommands)
		{
			_lobbyModule = lobbyModule;
			_logModule = logModule;
			_bus = bus;
			_creator = creator;
			_factory = factory;

			Modules = new ReactiveCollection<IModule>{_lobbyModule, _logModule};

			
			_bus.Listen<ActivateModuleMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg =>
					{
						CurrentModules = Modules.CreateDerivedCollection(module => new ModuleViewModel(module, bus));
					});
			
			bus.RegisterMessageSource(bus.Listen<ModuleLoaded>().Where(msg => msg.ModuleName == ModuleNames.MainCampfireView)
				.Select(_ => new ActivateModuleMessage(ModuleNames.MainCampfireView, _lobbyModule)));
			_activeModule = _lobbyModule;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(SyncModuleList);

			_bus.Listen<ActivateModuleByIdMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(HandleActivateModuleById);

			_bus.Listen<ModuleActivatedMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg => ActiveModule = msg.Module);

			ActivateModuleCommand = new ReactiveCommand();
			ActivateModuleCommand.OfType<ModuleViewModel>().Subscribe(HandleActivateModule);

			globalCommands.NextModuleCommand.Subscribe(OnNextModuleCommand);
			globalCommands.PreviousModuleCommand.Subscribe(OnPreviousModuleCommand);
		}

		private void OnPreviousModuleCommand(object o)
		{
			var index = Modules.IndexOf(_activeModule);
			if (index >= 0)
			{
				int nextIndex = index - 1 >= 0 ? index - 1: Modules.Count - 1;
				ActiveModule = Modules[nextIndex];
			}
		}

		private void OnNextModuleCommand(object o)
		{
			var index = Modules.IndexOf(_activeModule);
			if (index >= 0)
			{
				int nextIndex = (Modules.Count - 1) == index ? 0 : index + 1;
				ActiveModule = Modules[nextIndex];
			}
		}

		private void HandleActivateModuleById(ActivateModuleByIdMessage msg)
		{
			ActiveModule = CurrentModules.Select(m => m.Module).Where(m => m.Id == msg.Id).FirstOrDefault();
		}

		private IModule ActiveModule
		{
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
			var toRemove = Modules.Where(module => module != _lobbyModule && module != _logModule
				&& !roomPresenceMessage.IsPresentIn(module.Caption)).ToList();
			var toAdd = roomPresenceMessage.Rooms.Where(room =>
				!Modules.Any(m => m.Caption.Equals(room.Name, StringComparison.InvariantCultureIgnoreCase))).ToList();

			foreach (var module in toRemove)
			{
				Modules.Remove(module);
			}
			foreach (var room in toAdd)
			{
				var vm = _factory.Create(room);

				var newModule = _creator.CreateRoomModule(vm);

				int index = Modules.IndexOf(_logModule);
				Modules.Insert(index, newModule);
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
