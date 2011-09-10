using System;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomListViewModel : ReactiveObject
	{
		private readonly Room _room;
		private readonly IMessageBus _bus;
		private bool _isActive;

		public RoomListViewModel(Room room, IMessageBus bus)
		{
			_room = room;
			_bus = bus;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(msg => IsActive = msg.IsPresentIn(_room.Id));

			JoinCommand = new ReactiveCommand();
			JoinCommand.SubscribeUI(HandleJoinCommand);
		}

		private void HandleJoinCommand(object o)
		{
			if (IsActive)
			{
				_bus.SendMessage(new ActivateModuleByIdMessage(ModuleNames.MainCampfireView, _room.Id));
				return;
			}

			_bus.Listen<RoomModuleCreatedMessage>().Where(msg => msg.Module.Id == _room.Id).SubscribeOnceUI(
				msg => _bus.SendMessage(new ActivateModuleMessage(ModuleNames.MainCampfireView, msg.Module))
				);
			_bus.SendMessage(new RequestJoinRoomMessage(_room.Id));
		}

		public ReactiveCommand JoinCommand { get; private set; }

		public string Name
		{
			get { return _room.Name; }
		}

		public string Topic
		{
			get { return _room.Topic; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}
	}

}