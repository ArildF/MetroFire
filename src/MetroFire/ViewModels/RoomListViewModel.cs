using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomListViewModel : ReactiveObject
	{
		private readonly Room _room;
		private bool _isActive;

		public RoomListViewModel(Room room, IMessageBus bus)
		{
			_room = room;

			bus.Listen<RoomPresenceMessage>().SubscribeUI(msg => IsActive = msg.IsPresentIn(_room.Id));
		}

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