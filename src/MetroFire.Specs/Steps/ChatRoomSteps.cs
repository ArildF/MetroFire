using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
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
			_roomContext.SendMessage(new ActivateMainModuleMessage(ModuleNames.MainCampfireView));
		}

		[Given(@"that I have joined the room ""(.*)""")]
		public void GivenThatIHaveJoinedRoom(string roomName)
		{
			_roomContext.JoinRoom(roomName);
		}

		[When(@"the message ""(.*)"" is received for room ""(.*)""")]
		public void GivenThatTheMessageHelloWorldIsReceivedForRoomTest(string message, string roomName)
		{
			_roomContext.SendRoomMessage(message, roomName);
		}

		[Given(@"that the following messages are received for room ""(.*)"" in order:")]
		[When(@"the following messages are received for room ""(.*)"" in order:")]
		public void WhenTheFollowingMessagesAreReceivedInOrder(string roomName, Table table)
		{
			var messages = table.Rows.Select(r => new Message {Id =int.Parse(r["Id"]), Body = r["Message"]}).ToArray();
			_roomContext.SendRoomMessages(roomName, messages);

		}
		[When(@"the topic is changed to ""(.*)"" for room ""(.*)""")]
		public void WhenTheTopicIsChangedToToPicForRoomTest(string topic, string roomName)
		{
			var id = _roomContext.IdForRoom(roomName);
			_roomContext.SendMessage(new MessagesReceivedMessage(id, new []{new Message{Body = topic, RoomId = id, MessageTypeString = "TopicChangeMessage"}}, null));
		}

		[When(@"we wait (\d+) seconds")]
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


	}
}
