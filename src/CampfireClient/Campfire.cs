using System.Collections.Generic;
using Castle.Core;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public class Campfire : ICampfire, IStartable
	{
		private readonly IMessageBus _bus;
		private readonly ICampfireApi _api;

		private Account _account;
		private Room[] _rooms;
		private Room[] _presentRooms;


		public Campfire(IMessageBus bus, ICampfireApi api)
		{
			_bus = bus;
			_api = api;

		}

		public IAccount Account
		{
			get { return _account; }
		}

		public IEnumerable<Room> Rooms
		{
			get { return _rooms; }
		}

		private void Subscribe()
		{
			_bus.Listen<RequestLoginMessage>().SubscribeThreadPool(StartLogin);
			_bus.Listen<RequestRoomListMessage>().SubscribeThreadPool(ListRooms);
			_bus.Listen<RequestRoomPresenceMessage>().SubscribeThreadPool(ListRoomPresence);
		}

		private void ListRoomPresence(RequestRoomPresenceMessage obj)
		{
			_presentRooms = _api.ListPresence();
			_bus.SendMessage(new RoomPresenceMessage(_presentRooms));
		}

		private void ListRooms(RequestRoomListMessage obj)
		{
			_rooms = _api.ListRooms();
			_bus.SendMessage(new RoomListMessage(_rooms));
		}

		private void StartLogin(RequestLoginMessage requestLoginMessage)
		{
			_api.SetLoginInfo(requestLoginMessage.LoginInfo);
			_account = _api.GetAccountInfo();

			_bus.SendMessage<LoginSuccessfulMessage>(null);
		}

		public void Start()
		{
			Subscribe();
		}

		public void Stop()
		{
			
		}
	}
}