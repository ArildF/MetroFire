using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LobbyModuleViewModel : ReactiveObject, ILobbyModuleViewModel
	{
		private readonly IMessageBus _messageBus;
		private IAccount _account;
		private bool _isActive;

		public LobbyModuleViewModel(ICampfire campfire, IMessageBus messageBus)
		{
			_messageBus = messageBus;
			_account = campfire.Account;

			Rooms = new ReactiveCollection<RoomListViewModel>();

			_messageBus.Listen<AccountUpdated>()
				.SubscribeUI(msg =>
				{
					_account = msg.Account;
					this.RaisePropertyChanged(vm => vm.AccountName);
					this.RaisePropertyChanged(vm => vm.AccountPlan);
					this.RaisePropertyChanged(vm => vm.AccountStorage);
					this.RaisePropertyChanged(vm => vm.AccountSubdomain);
				});

			_messageBus.Listen<RoomListMessage>()
				.SubscribeUI(UpdateRooms);

			_messageBus.SendMessage(new RequestRoomListMessage());
		}

		public ReactiveCollection<RoomListViewModel> Rooms { get; private set; }

		private void UpdateRooms(RoomListMessage obj)
		{
			Rooms.Clear();
			foreach (var room in obj.Rooms)
			{
				Rooms.Add(new RoomListViewModel(room, _messageBus));
			}

			_messageBus.SendMessage(new RequestRoomPresenceMessage());
		}

		public string AccountName
		{
			get { return _account.Name; }
		}

		public string AccountPlan
		{
			get { return _account.Plan; }
		}
		public string AccountSubdomain
		{
			get { return _account.Subdomain; }
		}
		public string AccountStorage
		{
			get { return _account.Storage; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}
	}
}
