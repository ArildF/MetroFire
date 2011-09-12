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

	public class When_sending_a_request_room_presence_message_and_rooms_are_returned : CampfireContextBase
	{
		Establish context = () =>
			{
				_rooms = new Room[] {new Room(){Id = 42}};
				_api.WhenToldTo(a => a.ListPresence()).Return(_rooms);

				_bus.Listen<RoomPresenceMessage>().Subscribe(msg => _roomPresenceMessage = msg);
			};

		Because of = () => _bus.SendMessage<RequestRoomPresenceMessage>(null);

		It should_send_room_presence_message = () => _roomPresenceMessage.ShouldNotBeNull();
		It should_report_present_in_room_42 = () => _roomPresenceMessage.IsPresentIn(42).ShouldBeTrue();
		It should_not_report_present_in_room_101 = () => _roomPresenceMessage.IsPresentIn(101).ShouldBeFalse();
		It should_have_requested_presence = () => _api.WasToldTo(a => a.ListPresence());

		private static Room[] _rooms;
		private static RoomPresenceMessage _roomPresenceMessage;
	}

	public class When_sending_a_room_join_message : CampfireContextBase
	{
		Establish context = () =>
			{
				_bus.Listen<RoomPresenceMessage>().Subscribe(msg => _roomPresenceMessageSent = true);
				_bus.Listen<UserJoinedRoomMessage>().Subscribe(msg => _userJoinedRoomMessage = msg);
			};

		Because of = () => _bus.SendMessage(new RequestJoinRoomMessage(42));

		It should_have_joined_room = () => _api.WasToldTo(a => a.Join(42));
		It should_have_refreshed_presence = () => _roomPresenceMessageSent.ShouldBeTrue();
		It should_have_joined_room_42 = () => _userJoinedRoomMessage.Id.ShouldEqual(42);


		private static bool _roomPresenceMessageSent;
		private static UserJoinedRoomMessage _userJoinedRoomMessage;
	}

	public class When_sending_a_room_speak_message : CampfireContextBase
	{
		Establish context = () =>
		{
		};

		Because of = () => _bus.SendMessage(new RequestSpeakInRoomMessage(42, "Hello world"));

		It should_have_spoken_in_room_42 = () => _api.WasToldTo(a => a.Speak(42, "Hello world"));

		private static bool _roomPresenceMessageSent;
		private static UserJoinedRoomMessage _userJoinedRoomMessage;
	}

	public class When_sending_a_request_recent_messages_message : CampfireContextBase
	{
		Establish context = () =>
			{
				_msgs = new Message[]{};
				_api.WhenToldTo(a => a.GetMessages(42)).Return(_msgs);

				_bus.Listen<MessagesReceivedMessage>().Subscribe(msg => _messagesReceivedMessage = msg);
			};

		Because of = () => _bus.SendMessage(new RequestRecentMessagesMessage(42));

		It should_have_requested_messages = () => _api.WasToldTo(a => a.GetMessages(42));

		It should_have_sent_messages_received_message = () => _messagesReceivedMessage.ShouldNotBeNull();

		private It should_have_sent_the_messages = () => _messagesReceivedMessage.Messages.ShouldBeTheSameAs(_msgs);

		private static MessagesReceivedMessage _messagesReceivedMessage;
		private static Message[] _msgs;
	}

	public class When_sending_a_request_extended_room_information_message : CampfireContextBase
	{
		private Establish context = () =>
			{
				_room = new Room {Users = new User[] {}};
				_api.WhenToldTo(a => a.GetRoom(42)).Return(_room);

				_bus.Listen<RoomInfoReceivedMessage>().Subscribe(msg => _roomInfoReceivedMessage = msg);
			};

		Because of = () => _bus.SendMessage(new RequestRoomInfoMessage(42));

		It should_have_called_the_api = () => _api.WasToldTo(a => a.GetRoom(42));
		It should_have_sent_room_info_received_message = () => _roomInfoReceivedMessage.ShouldNotBeNull();
		It should_have_sent_the_room_with_the_room_info_received_message = () => _roomInfoReceivedMessage.Room.ShouldBeTheSameAs(_room);

		private static Room _room;
		private static RoomInfoReceivedMessage _roomInfoReceivedMessage;
	}

}
