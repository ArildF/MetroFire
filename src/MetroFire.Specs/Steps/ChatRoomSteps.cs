using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.ViewModels;
using TechTalk.SpecFlow;

namespace MetroFire.Specs.Steps
{
	[Binding]
	public class ChatRoomSteps
	{
		private readonly RoomContext _roomContext;

		public ChatRoomSteps(RoomContext roomContext)
		{
			_roomContext = roomContext;
		}

		[Given(@"a room called ""(.*)""")]
		public void GivenARoomCalledTest(string roomName)
		{
			_roomContext.CreateRoom(roomName);
		}

		[Given(@"that I am logged in")]
		public void GivenThatIAmLoggedIn()
		{
			_roomContext.LoginViewModel.Token = "12345";
			_roomContext.ApiFake.CorrectToken = "12345";
			_roomContext.LoginViewModel.LoginCommand.Execute(null);
		}

		[Given(@"a user '(.*)'")]
		public void GivenAUserTestuser(string user)
		{
			_roomContext.AddUser(user);
		}



		[Given(@"that room ""(.*)"" is the active module")]
		public void GivenThatRoomTestIsTheActiveModule(string roomName)
		{
			_roomContext.ActivateModule(roomName);
		}

		[Given(@"that I have joined the room ""(.*)""")]
		public void GivenThatIHaveJoinedRoom(string roomName)
		{
			_roomContext.JoinRoom(roomName);
		}

		[When(@"I click the leave room button in room ""(.*)""")]
		public void WhenIClickTheLeaveRoomButton(string roomName)
		{
			var vm = _roomContext.ViewModelFor(roomName);
			GlobalCommands.LeaveRoomCommand.Execute(vm.Id);
		}

		[When(@"I send the message ""(.*)"" to room ""(.*)""")]
		public void WhenISendTheMessageHelloWorldToRoomTest(string message, string roomName)
		{
			var vm = _roomContext.ViewModelFor(roomName);
			vm.UserMessage = message;
			vm.PostMessageCommand.Execute(null);
		}

		[Given(@"the message ""(.*)"" is received from user '(.*)' for room ""(.*)""")]
		[When(@"the message ""(.*)"" is received from user '(.*)' for room ""(.*)""")]
		public void GivenThatTheMessageHelloWorldIsReceivedForRoomTest(string message, string username, string roomName)
		{
			_roomContext.SendRoomMessage(username, message, roomName);
		}

		[Given(@"that the following messages are received from user '(.*)' for room ""(.*)"" in order:")]
		[When(@"the following messages are received from user '(.*)' for room ""(.*)"" in order:")]
		public void WhenTheFollowingMessagesAreReceivedInOrder(string userName, string roomName, Table table)
		{
			var user = _roomContext.GetUser(userName);

			var messages = table.Rows.Select(r => new Message {Id =int.Parse(r["Id"]), 
				UserIdString =  user.Id.ToString(CultureInfo.InvariantCulture),
				Body = r["Message"], Type = MessageType.TextMessage}).ToArray();
			_roomContext.SendRoomMessages(roomName, messages);

		}
		[When(@"the topic is changed to ""(.*)"" for room ""(.*)""")]
		public void WhenTheTopicIsChangedToToPicForRoomTest(string topic, string roomName)
		{
			var id = _roomContext.IdForRoom(roomName);
			_roomContext.SendMessage(new MessagesReceivedMessage(id, new []{new Message{Body = topic, RoomId = id, MessageTypeString = "TopicChangeMessage"}}, null));
		}

		[When(@"we wait (\d+) seconds")]
		[When(@"I wait (\d+) seconds")]
		public void WhenWeWait12Seconds(int secs)
		{
			Events.TestScheduler.AdvanceBy(TimeSpan.FromSeconds(secs).Ticks);
		}

		[When(@"the streaming is (.*) for room ""(.*)""")]
		public void WhenTheStreamingIsDisconnectedForRoomTest(string state, string roomName)
		{
			bool connected = state == "reconnected";
			_roomContext.ApiFake.SetStreamingConnectionState(roomName, connected);
		}

		[Then(@"the message ""(.*)"" should be displayed in room ""(.*)""")]
		public void ThenTheMessageHelloWorldShouldBeDisplayedInRoomTest(string message, string roomName)
		{
			_roomContext.MessagesDisplayedInRoom(roomName).Last().Body.Should().Be(message);
		}

		[Then(@"the following messages should be displayed in room ""(.*)"" in order:")]
		public void ThenTheFollowingMessagesShouldBeDisplayedInRoomTestInOrder(string roomName, Table table)
		{
			var messageBodies = table.Rows.Select(r => r["Message"]);
			_roomContext.MessagesDisplayedInRoom(roomName).Select(m => m.Body).Should().BeEquivalentTo(messageBodies);
		}

		[Then(@"I should leave room ""(.*)""")]
		public void ThenIShouldLeaveRoomTest(string roomName)
		{
			_roomContext.ApiFake.JoinedRooms.Should().NotContain(r => r.Name == roomName);
		}

		[Then(@"the topic should be ""(.*)"" for room ""(.*)""")]
		public void ThenTheTopicShouldBeToPicForRoomTest(string topic, string room)
		{
			_roomContext.ViewModelFor(room).Topic.Should().Be(topic);
		}

		[Then(@"room ""(.*)"" should show that it is (.*)")]
		public void ThenRoomTestShouldShowThatItIsDisconnected(string roomName, string state)
		{
			bool connected = state == "connected";
			_roomContext.ViewModelFor(roomName).IsConnected.Should().Be(connected);
		}

		[Then(@"older messages should have been requested for room ""(.*)""")]
		public void ThenShouldRequestAllOlderMessages(string roomName)
		{
			var id = _roomContext.IdForRoom(roomName);
			_roomContext.AllMessages.OfType<RequestRecentMessagesMessage>().Where(msg => msg.RoomId == id).
				Skip(1).Should().NotBeEmpty();
		}

		[Then(@"the lobby should be active")]
		public void ThenTheLobbyShouldBeActive()
		{
			_roomContext.MainViewModel.ActiveModule.Should().BeOfType<LobbyModuleViewModel>();
		}


		[Then(@"streaming should be disconnected for room ""(.*)""")]
		public void ThenStreamingShouldBeDisconnectedForRoomTest(string roomName)
		{
			var id = _roomContext.IdForRoom(roomName);
			_roomContext.ApiFake.Streamers().Should().NotContain(s => s.Id == id);
		}

	}
}
