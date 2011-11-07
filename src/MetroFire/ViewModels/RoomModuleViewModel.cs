using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
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
		private readonly IClipboard _clipboard;
		private readonly List<RoomMessage> _messages;
		private int? _sinceMessageId;
		private int _notificationCount;

		private bool _streamingStarted;
		private bool _isConnected;

		public RoomModuleViewModel(IRoom room, IMessageBus bus,IUserCache userCache, IChatDocument chatDocument,
			IClipboard clipboard, IImageEncoder encoder)
		{
			_room = room;
			_bus = bus;
			_userCache = userCache;
			_chatDocument = chatDocument;
			_clipboard = clipboard;

			Users = new ReactiveCollection<UserViewModel>();

			UserMessage = DefaultMessage;

			PostMessageCommand = new ReactiveCommand(this.ObservableForProperty(vm => vm.UserEditedMessage)
				.Select(c => c.Value).StartWith(false));
			PostMessageCommand.Subscribe(HandlePostMessage);

			_messages = new List<RoomMessage>();


			_bus.RegisterMessageSource(
				_bus.Listen<ConnectionState>().Where(msg => msg.RoomId == room.Id)
					.Do(cs => IsConnected = cs.Connected)
					.Where(cs => cs.Connected)
					.Delay(TimeSpan.FromSeconds(10), RxApp.TaskpoolScheduler)
					.Where(_ => _streamingStarted && IsConnected)
					.Select(_ => new RequestRecentMessagesMessage(_room.Id)));
				;
			_bus.Listen<MessagesReceivedMessage>().Where(msg => msg.RoomId == room.Id).SubscribeUI(HandleMessagesReceived);
			_bus.Listen<RoomInfoReceivedMessage>().Where(msg => msg.Room.Id == _room.Id).SubscribeUI(HandleRoomInfoReceived);
			_bus.Listen<UsersUpdatedMessage>().SubscribeUI(HandleUsersUpdated);

			_bus.SendMessage(new RequestRecentMessagesMessage(room.Id));
			_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));

			_bus.RegisterMessageSource(Observable.Interval(TimeSpan.FromMinutes(5)).Select(
				_ => new RequestKeepAliveMessage(_room.Id)));

			PasteCommand = new ReactiveCommand();

			PasteCommand.Select(pc => _clipboard.GetImage())
				.Select(encoder.EncodeToTempPng)
				.Subscribe(path => _chatDocument.AddPasteFile(_room, path));
		}


		public ReactiveCommand PasteCommand { get; private set; }

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
			bool isInitialLoad = _sinceMessageId == null;
			var newMessages = obj.Messages.Except(_messages.Select(m => m.Message), MessageEqualityComparer.Default)
				.ToArray();
			foreach (var message in newMessages)
			{
				var existingUser = Users.Select(u => u.User).FirstOrDefault(u => u.Id == message.UserId);
				User user = message.UserId != null ? _userCache.GetUser(message.UserId.GetValueOrDefault(), existingUser) : null;
				var after = _messages.LastOrDefault(m => m.Message.Id < message.Id);

				var textObject = _chatDocument.AddMessage(message, user, after != null ? after.TextObject : null);

				var roomMessage = new RoomMessage(message, user, textObject);
				if (after != null)
				{
					int index = _messages.IndexOf(after);
					_messages.Insert(index + 1, roomMessage);
				}
				else
				{
					_messages.Add(roomMessage);
				}
				_sinceMessageId = message.Id;
			}

			if (newMessages.Any(msg => msg.Type.In(MessageType.EnterMessage, MessageType.KickMessage, MessageType.LeaveMessage)))
			{
				_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));
			}

			var count = newMessages.Count(msg => msg.Type.In(MessageType.PasteMessage, MessageType.TextMessage));
			if (count > 0)
			{
				_bus.SendMessage(new RoomActivityMessage(_room.Id, count));
			}
			if (!IsActive)
			{
				NotificationCount += count;
			}
			if (!_streamingStarted)
			{
				_bus.SendMessage(new RequestStartStreamingMessage(_room.Id));
				_streamingStarted = true;
			}

			if (isInitialLoad)
			{
				_bus.SendMessage(new RoomBackLogLoadedMessage(_room.Id));
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

		public string Topic
		{
			get { return _room.Topic; }
		}

		public bool IsConnected
		{
			get { return _isConnected; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsConnected, ref _isConnected, value); }
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

		private class MessageEqualityComparer : IEqualityComparer<Message>
		{
			public static readonly MessageEqualityComparer Default = new MessageEqualityComparer();

			public bool Equals(Message x, Message y)
			{
				return x.Id == y.Id;
			}

			public int GetHashCode(Message obj)
			{
				return obj.Id.GetHashCode();
			}
		}
	}

	public class PasteItem
	{
		public PasteItem(BitmapSource bitmapSource)
		{
			
		}
	}
}
