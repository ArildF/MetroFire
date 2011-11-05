using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.ViewModels;

namespace MetroFire.Specs.Steps
{
	public class RoomContext
	{
		private readonly Fixture _fixture;
		private readonly Dictionary<string, RoomModuleViewModel> _rooms = new Dictionary<string, RoomModuleViewModel>();
		private int _currentRoomId;
		private readonly IMessageBus _bus;
		private readonly ChatViewFake _chatViewFake ;

		public RoomContext()
		{
			_fixture = new Fixture();
			_fixture.Customize(new AutoMoqCustomization());
			_bus = new MessageBus();
			_fixture.Inject(_bus);
			_chatViewFake = new ChatViewFake();
			_fixture.Inject<IChatDocument>(_chatViewFake);

		}

		public void CreateRoom(string roomName)
		{
			_fixture.Freeze<IRoom>(new Room {Id =_currentRoomId++, Name = roomName});
			_rooms.Add(roomName, _fixture.CreateAnonymous<RoomModuleViewModel>());
		}

		public void SendMessage<T>(string roomName, T message)
		{
			_bus.SendMessage(message);
		}

		public int IdForRoom(string roomName)
		{
			return _rooms[roomName].Id;
		}

		public IEnumerable<Message> MessagesForRoom(string roomName)
		{
			return _chatViewFake.Messages;
		}
	}
}
