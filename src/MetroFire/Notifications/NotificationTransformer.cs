using System.Collections.Generic;
using Castle.Core;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using System;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.Notifications
{
	public class NotificationTransformer : IStartable
	{
		private readonly IMessageBus _bus;
		private readonly IUserCache _userCache;

		private readonly Dictionary<int, Room> _rooms = new Dictionary<int, Room>();
		private readonly ISet<int> _loadedRooms = new SortedSet<int>();
		private int _currentUserId;


		public NotificationTransformer(IMessageBus bus, IUserCache userCache)
		{
			_bus = bus;
			_userCache = userCache;
		}

		public void Start()
		{
			_bus.Listen<NewMessagesReceivedMessage>().Subscribe(MessagesReceived);
			_bus.Listen<RoomInfoReceivedMessage>().Subscribe(RoomInfoReceived);
			_bus.Listen<RoomBackLogLoadedMessage>().Subscribe(msg => _loadedRooms.Add(msg.RoomId));
			_bus.Listen<CurrentUserInformationReceivedMessage>().Subscribe(msg => _currentUserId = msg.User.Id);
		}

		private void RoomInfoReceived(RoomInfoReceivedMessage roomInfoReceivedMessage)
		{
			_rooms[roomInfoReceivedMessage.Room.Id] = roomInfoReceivedMessage.Room;
		}

		private void MessagesReceived(NewMessagesReceivedMessage messagesReceivedMessage)
		{
			Room room;
			if (!_rooms.TryGetValue(messagesReceivedMessage.RoomId, out room) || !_loadedRooms.Contains(messagesReceivedMessage.RoomId))
			{
				return;
			}
			foreach (var message in messagesReceivedMessage.Messages)
			{
				var user = _userCache.GetUser(message.UserId ?? -1);
				if (user == null || user.Id == _currentUserId)
				{
					continue;
				}
				_bus.SendMessage(new NotificationMessage(room, user, message));
			}
		}

		public void Stop()
		{
		}
	}
}
