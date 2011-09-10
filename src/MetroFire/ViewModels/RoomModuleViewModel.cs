using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomModuleViewModel : ReactiveObject, IRoomModuleViewModel
	{
		private readonly IRoom _room;
		private bool _isActive;

		public RoomModuleViewModel(IRoom room)
		{
			_room = room;
		}

		public string RoomName
		{
			get { return _room.Name; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}

		public int Id
		{
			get { return _room.Id; }
		}
	}

}
