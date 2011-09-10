using Castle.Core;
using Machine.Fakes;
using Machine.Specifications;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using System;

namespace Rogue.MetroFire.CampfireClient.Specs
{
	public class CampfireContextBase : WithFakes
	{
		Establish context = () =>
		{
			_api = An<ICampfireApi>();
			_bus = new MessageBus();

			_campfire = new Campfire(_bus, _api);
			((IStartable)_campfire).Start();
		};

		protected static ICampfire _campfire;
		protected static MessageBus _bus;
		protected static ICampfireApi _api;

	}

	[Subject(typeof(Campfire))]
	public class When_sending_a_request_login_message_and_login_is_successful : CampfireContextBase
	{
		Establish context = () =>
								{
									_account = new Account();
									_api.WhenToldTo(a => a.GetAccountInfo()).Return(_account);

									_bus.Listen<LoginSuccessfulMessage>().Subscribe(msg => _loginSuccessfulMessageSent = true);
								};

		Because of = () => _bus.SendMessage(new RequestLoginMessage(_loginInfo));

		It should_set_login_info = () => _api.WasToldTo(a => a.SetLoginInfo(_loginInfo));
		It should_request_account_info = () => _api.WasToldTo(a => a.GetAccountInfo());

		It should_send_login_successful_message = () => _loginSuccessfulMessageSent.ShouldBeTrue();

		It should_expose_account = () => _campfire.Account.ShouldBeTheSameAs(_account);


		private static readonly LoginInfo _loginInfo = new LoginInfo("", "");
		private static Account _account;
		private static bool _loginSuccessfulMessageSent;
	}

	public class When_sending_a_request_room_message_and_rooms_are_returned : CampfireContextBase
	{
		Establish context = () =>
		{
			_rooms = new Room[] { };
			_api.WhenToldTo(a => a.ListRooms()).Return(_rooms);

			_bus.Listen<RoomListMessage>().Subscribe(_ => _roomListMessageSent = true);
		};

		Because of = () => _bus.SendMessage<RequestRoomListMessage>(null);

		It should_send_room_list_message = () => _roomListMessageSent.ShouldBeTrue();
		It should_expose_rooms = () => _campfire.Rooms.ShouldBeTheSameAs(_rooms);
		It should_request_rooms = () => _api.WasToldTo(api => api.ListRooms());



		private static Room[] _rooms;
		private static bool _roomListMessageSent;
	}

}
