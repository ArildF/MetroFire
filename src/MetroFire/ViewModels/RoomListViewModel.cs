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
		private bool _isCurrentRoom;
		private int _roomActivityCount;
		private string _notifications;
		private bool _isJoiningRoom;

		public RoomListViewModel(Room room, IMessageBus bus)
		{
			_room = room;
			_bus = bus;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(msg => IsActive = msg.IsPresentIn(_room.Id));

			_bus.Listen<RoomActivatedMessage>().Where(msg => msg.RoomId == _room.Id)
				.Do(_ => IsJoiningRoom = false)
				.SubscribeUI(_ => IsCurrentRoom = true);

			_bus.Listen<RoomDeactivatedMessage>().Where(msg => msg.RoomId == _room.Id)
				.SubscribeUI(_ => IsCurrentRoom = false);

			_bus.Listen<RoomActivityMessage>().Where(msg => msg.RoomId == _room.Id)
				.SubscribeUI(msg => RoomActivityCount += msg.Count);

			JoinCommand = new ReactiveCommand();
			JoinCommand.Do(_ => IsJoiningRoom = true).SubscribeUI(HandleJoinCommand);
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


		public bool IsCurrentRoom
		{
			get { return _isCurrentRoom; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsCurrentRoom, ref _isCurrentRoom, value);
				if (value)
				{
					RoomActivityCount = 0;
				}
			}
		}

		public string Notifications
		{
			get { return _notifications; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Notifications, ref _notifications, value); }
		}

		public bool IsJoiningRoom
		{
			get { return _isJoiningRoom; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsJoiningRoom, ref _isJoiningRoom, value); }
		}

		private int RoomActivityCount
		{
			get { return _roomActivityCount; }
			set
			{
				_roomActivityCount = value;
				Notifications = _roomActivityCount > 0 ? _roomActivityCount.ToString() : "";
			}

		}

	}

}