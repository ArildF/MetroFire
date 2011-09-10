using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomListViewModel : ReactiveObject
	{
		private readonly Room _room;

		public RoomListViewModel(Room room)
		{
			_room = room;
		}

		public string Name
		{
			get { return _room.Name; }
		}

		public string Topic
		{
			get { return _room.Topic; }
		}
	}
}