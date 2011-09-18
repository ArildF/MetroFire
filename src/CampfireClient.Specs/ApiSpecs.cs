using System;
using System.IO;
using System.Linq;
using Machine.Specifications;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient.Specs
{
	public class ApiContext
	{
		private Establish context =
			() => api = CreateApi();

		private static CampfireApi CreateApi()
		{
			var lines = File.ReadAllLines("Creds.txt");
			var url = lines[0];
			var token = lines[1];

			var api = new CampfireApi();
			api.SetLoginInfo(new LoginInfo(url, token));

			return api;
		}
		protected static CampfireApi api;

		protected static int FindJoinedRoom()
		{
			var presence = api.ListPresence();
			if (!presence.Any())
			{
				api.Join(api.ListRooms().First().Id);
				presence = api.ListPresence();
			}

			return presence.First().Id;
		}

		protected static int FindNotJoinedRoom()
		{
			var rooms = api.ListRooms();
			var presence = api.ListPresence();
			if (presence.Length == rooms.Length)
			{
				api.Leave(rooms.First().Id);
				presence = api.ListPresence();
			}

			return rooms.First(r => !presence.Any(p => p.Id == r.Id)).Id;
		}
	}
	[Subject(typeof(Account))]
	public class When_retrieving_account_info : ApiContext
	{

		Because of = () => account = api.GetAccountInfo();

		It should_contain_data = () => account.CreatedAt.Year.ShouldEqual(2011);

		private static Account account;
	}

	[Subject(typeof(Room))]
	public class When_retrieving_room_list : ApiContext
	{
		Because of = () => rooms = api.ListRooms();

		It should_contain_list_of_rooms = () => rooms.ShouldNotBeEmpty();

		private static Account account;
		private static Room[] rooms;
	}

	[Subject(typeof(Room))]
	public class When_retrieving_presence_list : ApiContext
	{
		Because of = () => rooms = api.ListPresence();

		It should_return_list_of_rooms = () => rooms.ShouldNotBeNull();

		private static Room[] rooms;
	}

	public class When_leaving_room : ApiContext
	{
		Establish context = () =>
			{
				var rooms = api.ListRooms();
				var presence = api.ListPresence();
				if (presence.Length == 0)
				{
					api.Join(rooms.First().Id);
					presence = api.ListPresence();
				}

				_roomIdToLeave = presence.First().Id;
			};

		Because of = () => api.Leave(_roomIdToLeave);

		It should_not_be_in_the_room = () => api.ListPresence().ShouldNotContain(r => r.Id == _roomIdToLeave);


		private static int _roomIdToLeave;
	}

	public class When_joining_room : ApiContext
	{
		Establish context = () =>
		{
			_roomIdToJoin = FindNotJoinedRoom();
		};

		Because of = () => api.Join(_roomIdToJoin);

		It should_be_in_the_room = () => api.ListPresence().ShouldContain(r => r.Id == _roomIdToJoin);

		private static int _roomIdToJoin;
	}

	public class When_retrieving_messages : ApiContext
	{
		Establish context = () =>
			{
				_idToGetMessagesFrom = FindJoinedRoom();
			};

		Because of = () => _messages = api.GetMessages(_idToGetMessagesFrom);

		It should_contain_messages = () => _messages.ShouldNotBeEmpty();


		private static Message[] _messages;
		private static int _idToGetMessagesFrom;
	}

	public class When_retrieving_messages_with_a_since_id : ApiContext
	{
		Establish context = () =>
			{
				_idToGetMessagesFrom = FindJoinedRoom();
				var messages = api.GetMessages(_idToGetMessagesFrom);
				_sinceId = messages.Reverse().Skip(4).First().Id;
			};

		Because of = () => _messages = api.GetMessages(_idToGetMessagesFrom, _sinceId);

		It should_only_contain_four_messages = () => _messages.Count().ShouldEqual(4);

		private static int _idToGetMessagesFrom;
		private static int _sinceId;
		private static Message[] _messages;
	}


	public class When_posting_speak_message : ApiContext
	{
		private Establish context = () =>
			{
				_idToPostTo = FindJoinedRoom();
				_guid = Guid.NewGuid();
			};

		Because of = () => _msg = api.Speak(_idToPostTo, "Automated test message " + _guid.ToString());

		It should_be_posted_in_the_room =
			() => api.GetMessages(_idToPostTo).Last().Body.ShouldEqual("Automated test message " + _guid.ToString());

		It should_have_returned_the_posted_message = () => _msg.Body.ShouldEqual("Automated test message " + _guid.ToString());

		It should_have_a_text_message_as_the_posted_message = () => _msg.Type.ShouldEqual(MessageType.TextMessage);

		private static int _idToPostTo;
		private static Guid _guid;
		private static Message _msg;
	}

	public class When_requesting_extended_room_information : ApiContext
	{
		Establish context = () =>
			{
				_idToRequest = FindJoinedRoom();
			};

		Because of = () => _room = api.GetRoom(_idToRequest);

		It should_contain_user_information = () => _room.Users.ShouldNotBeNull();
		It should_contain_at_least_one_user = () => _room.Users.Length.ShouldBeGreaterThan(0);
		It should_contain_metro_fire = () => _room.Users.ShouldContain(user => user.Name == "Metro Fire");
		It should_have_user_ids = () => _room.Users.ShouldEachConformTo(user => user.Id > 0);

		private static int _idToRequest;
		private static Room _room;

	}

	public class When_requesting_user_information : ApiContext
	{
		Establish context = () =>
		{
			var roomId = FindJoinedRoom();
			var messages = api.GetMessages(roomId);
			_userId = messages.Select(msg => msg.UserId).First(userId => userId != null);
		};

		Because of = () => _user = api.GetUser((int)_userId);

		It should_contain_user_information = () => _user.ShouldNotBeNull();
		It should_contain_user_name = () => _user.Name.ShouldNotBeNull();

		private static int _idToRequest;
		private static User _user;
		private static int? _userId;
	}
}
