using System;
using System.Collections.Generic;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;
using System.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomModuleViewModel : ReactiveObject, IRoomModuleViewModel
	{
		private IRoom _room;
		private readonly IMessageBus _bus;
		private readonly IUserCache _userCache;
		private bool _isActive;
		private bool _userEditingMessage;

		private const string DefaultMessage = "Type your message here or paste/drop files\u00A0";
		private string _userMessage;
		private bool _userEditedMessage;
		private readonly IChatDocument _chatDocument;
		private readonly List<RoomMessage> _messages;
		private int? _sinceMessageId;
		private int _notificationCount;

		public RoomModuleViewModel(IRoom room, IMessageBus bus,IUserCache userCache, IChatDocument chatDocument)
		{
			_room = room;
			_bus = bus;
			_userCache = userCache;
			_chatDocument = chatDocument;

			Users = new ReactiveCollection<UserViewModel>();

			UserMessage = DefaultMessage;

			PostMessageCommand = new ReactiveCommand(this.ObservableForProperty(vm => vm.UserEditedMessage)
				.Select(c => c.Value).StartWith(false));
			PostMessageCommand.Subscribe(HandlePostMessage);

			_messages = new List<RoomMessage>();


			_bus.Listen<MessagesReceivedMessage>().Where(msg => msg.RoomId == room.Id).SubscribeUI(HandleMessagesReceived);
			_bus.Listen<RoomInfoReceivedMessage>().Where(msg => msg.Room.Id == _room.Id).SubscribeUI(HandleRoomInfoReceived);
			_bus.Listen<UsersUpdatedMessage>().SubscribeUI(HandleUsersUpdated);

			_bus.SendMessage(new RequestRecentMessagesMessage(room.Id));
			_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));

			_bus.RegisterMessageSource(
				Observable.Interval(TimeSpan.FromSeconds(20))
					.Where(_ => _sinceMessageId != null)
					.Select(_ => new RequestRecentMessagesMessage(_room.Id, _sinceMessageId))
				);
		}

		private void HandleUsersUpdated(UsersUpdatedMessage obj)
		{
			foreach (var msg in _messages.Where(m => m.TextObject != null))
			{
				foreach (var user in obj.UsersToUpdate)
				{
					if (msg.UserId == user.Id)
					{
						msg.User = user;
						_chatDocument.UpdateMessage(msg.TextObject, msg.Message, user);
					}
				}
			}
		}

		public ReactiveCollection<UserViewModel> Users { get; private set; }

		private void HandleRoomInfoReceived(RoomInfoReceivedMessage obj)
		{
			_room = obj.Room;
			if (_room.Users != null)
			{
				PopulateUsers(_room.Users);
			}
		}

		private void PopulateUsers(IEnumerable<User> users)
		{
			Users.Clear();
			foreach (var vm in users.Select(u => new UserViewModel(u)))
			{
				Users.Add(vm);
			}
		}

		private void HandleMessagesReceived(MessagesReceivedMessage obj)
		{
			var messages = obj.Messages.Where(msg => obj.SinceMessageId == null || msg.Id > _sinceMessageId).ToList();
			foreach (var message in messages)
			{
				var existingUser = Users.Select(u => u.User).FirstOrDefault(u => u.Id == message.UserId);
				User user = message.UserId != null ? _userCache.GetUser(message.UserId.GetValueOrDefault(), existingUser) : null;
				var textObject = _chatDocument.AddMessage(message, user);

				_messages.Add(new RoomMessage(message, user, textObject));
				_sinceMessageId = message.Id;
			}

			if (obj.Messages.Any(msg => msg.Type.In(MessageType.EnterMessage, MessageType.KickMessage, MessageType.LeaveMessage)))
			{
				_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));
			}

			var count = obj.Messages.Count(msg => msg.Type.In(MessageType.PasteMessage, MessageType.TextMessage));
			if (count > 0)
			{
				_bus.SendMessage(new RoomActivityMessage(_room.Id, count));
			}
			if (!IsActive)
			{
				NotificationCount += count;
			}
		}

		private void HandlePostMessage(object o)
		{
			_bus.SendMessage(new RequestSpeakInRoomMessage(Id, UserMessage));
			UserMessage = String.Empty;
		}

		private int NotificationCount
		{
			get { return _notificationCount; }
			set
			{
				if (_notificationCount == value)
				{
					return;
				}
				_notificationCount = value;
				this.RaisePropertyChanged(vm => vm.Notifications);
			}
		}

		public IChatDocument ChatDocument
		{
			get{return _chatDocument;}
		}

		public ReactiveCommand PostMessageCommand { get; set; }

		public string RoomName
		{
			get { return _room.Name; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (value == _isActive)
				{
					return;
				}
				this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value);
				if (_isActive)
				{
					NotificationCount = 0;
					_bus.SendMessage(new RoomActivatedMessage(_room.Id));
				}
				else
				{
					_bus.SendMessage(new RoomDeactivatedMessage(_room.Id));
				}
			}
		}

		public string UserMessage
		{
			get { return _userMessage; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.UserMessage, ref _userMessage, value);
				UserEditedMessage = value != DefaultMessage && !String.IsNullOrEmpty(value);
			}
		}

		public bool UserEditingMessage
		{
			get { return _userEditingMessage; }
			set
			{
				if (value && UserMessage == DefaultMessage)
				{
					UserMessage = string.Empty;
				}
				if (!value && String.IsNullOrEmpty(UserMessage))
				{
					UserMessage = DefaultMessage;
				}
				this.RaiseAndSetIfChanged(vm => vm.UserEditingMessage, ref _userEditingMessage, value);
			}
		}

		public bool UserEditedMessage
		{
			get { return _userEditedMessage; }
			set { this.RaiseAndSetIfChanged(vm => vm.UserEditedMessage, ref _userEditedMessage, value); }
		}

		public int Id
		{
			get { return _room.Id; }
		}

		public string Notifications
		{
			get { return _notificationCount > 0 ? _notificationCount.ToString() : ""; }
		}

		private class RoomMessage
		{
			public RoomMessage(Message message, User user, object textObject)
			{
				User = user;
				TextObject = textObject;
				Message = message;
			}

			public Message Message { get; private set; }
			public int? UserId { get { return Message.UserId; } }
			public User User { get; set; }
			public object TextObject { get; private set; }
		}
	}
}
