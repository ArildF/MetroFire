using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomModuleViewModel : IRoomModuleViewModel
	{
		private readonly IRoom _room;

		public RoomModuleViewModel(IRoom room)
		{
			_room = room;
		}

		public string RoomName
		{
			get { return _room.Name; }
		}
	}

}
