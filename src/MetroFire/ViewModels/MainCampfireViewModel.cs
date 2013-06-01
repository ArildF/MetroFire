using System;
using System.Linq;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class MainCampfireViewModel : ReactiveObject, IMainCampfireViewModel, IMainModule
	{
		private readonly ILobbyModule _lobbyModule;
		private readonly IMessageBus _bus;
		private readonly IModuleCreator _creator;
		private IModule _activeModule;
		private readonly IModule _logModule;

		public MainCampfireViewModel(ILobbyModule lobbyModule, ILogModule logModule, IMessageBus bus, 
			IModuleCreator creator, IGlobalCommands globalCommands, INavigationContentViewModel navigationContentViewModel)
		{
			_lobbyModule = lobbyModule;
			_logModule = logModule;
			_bus = bus;
			_creator = creator;

			NavigationContent = navigationContentViewModel;

			Modules = new ReactiveCollection<IModule>{_lobbyModule, _logModule};

			
			_bus.Listen<ActivateModuleMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg =>
					{
						ActiveModule = msg.Module;
					});
			
			bus.RegisterMessageSource(bus.Listen<ModuleLoaded>().Where(msg => msg.ModuleName == ModuleNames.MainCampfireView)
				.Select(_ => new ActivateModuleMessage(ModuleNames.MainCampfireView, _lobbyModule)));
			_activeModule = _lobbyModule;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(SyncModuleList);

			_bus.Listen<ActivateModuleByIdMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(HandleActivateModuleById);

			_bus.Listen<ModuleActivatedMessage>().Where(msg => msg.ParentModule == ModuleNames.MainCampfireView)
				.SubscribeUI(msg => 
				{ 
					ActiveModule = msg.Module;
				});

			_bus.Listen<RequestLeaveRoomMessage>().SubscribeUI(OnRequestLeaveRoomMessage);

			globalCommands.NextModuleCommand.Subscribe(OnNextModuleCommand);
			globalCommands.PreviousModuleCommand.Subscribe(OnPreviousModuleCommand);
		}

		public Direction ModuleDirectionRelativeTo(IModule current, IModule module)
		{
			int indexCurrent = Modules.IndexOf(current);
			int indexModule = Modules.IndexOf(module);

			return indexCurrent >= indexModule ? Direction.Left : Direction.Right;
		}

		private void OnRequestLeaveRoomMessage(RequestLeaveRoomMessage requestLeaveRoomMessage)
		{
			var module = FindModuleById(requestLeaveRoomMessage.Id);
			if (module != null)
			{
				Modules.Remove(module);
				_creator.ReleaseModule(module);
			}
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
			ActiveModule = FindModuleById(msg.Id);
		}

		private IModule FindModuleById(int id)
		{
			return Modules.FirstOrDefault(m => m.Id == id);
		}

		public IModule  ActiveModule
		{
			get { return _activeModule; }
			set
			{
				if (_activeModule == value)
				{
					return;
				}

				if (_activeModule != null)
				{
					_activeModule.IsActive = false;
				}
				
				this.RaiseAndSetIfChanged(vm => vm.ActiveModule, ref _activeModule, value);
				if (_activeModule != null)
				{
					_activeModule.IsActive = true;
				}
				_bus.SendMessage(new ModuleActivatedMessage(_activeModule, ModuleNames.MainCampfireView));
			}
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

				if (module == _activeModule)
				{
					ActiveModule = _lobbyModule;
				}

				_creator.ReleaseModule(module);
			}
			foreach (var room in toAdd)
			{
				var newModule = _creator.CreateRoomModule(room);

				int index = Modules.IndexOf(_logModule);
				Modules.Insert(index, newModule);
			}
		}


		public ReactiveCollection<IModule> Modules { get;  private set; }
		public object NavigationContent { get; private set; }
	}
}
