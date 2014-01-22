﻿using System;
using System.Collections.Generic;
using System.Globalization;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;
using System.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomModuleViewModel : ViewModelBase, IModule
	{
		private IRoom _room;
		private readonly IMessageBus _bus;
		private readonly IUserCache _userCache;
		private bool _isActive;

		private string _userMessage;
		private readonly IChatDocument _chatDocument;
		private readonly ISettings _settings;
		private readonly List<RoomMessage> _messages;
		private int? _sinceMessageId;
		private int _notificationCount;

		private bool _streamingStarted;
		private bool _isConnected;
		private string _topic;
		private bool _userEditedMessage;
		private string _roomTranscriptsUri;

		public RoomModuleViewModel(IRoom room, IMessageBus bus,IUserCache userCache, IChatDocument chatDocument,
			IClipboardService clipboardService , IGlobalCommands commands, ISettings settings)
		{
			_room = room;
			_bus = bus;
			_userCache = userCache;
			_chatDocument = chatDocument;
			_settings = settings;


			Topic = _room.Topic;

			Users = new ReactiveCollection<UserViewModel>();

			UserMessage = String.Empty;

			PostMessageCommand = new ReactiveCommand(this.ObservableForProperty(vm => vm.UserEditedMessage)
				.Select(c => c.Value).StartWith(false));
			Subscribe(() => PostMessageCommand.Subscribe(HandlePostMessage));

			_messages = new List<RoomMessage>();


			Subscribe(() => _bus.RegisterMessageSource(
				_bus.Listen<ConnectionState>()
					.Where(msg => msg.RoomId == room.Id)
					.Do(cs => IsConnected = cs.Connected)
					.Where(cs => cs.Connected)
					.Delay(TimeSpan.FromSeconds(10), RxApp.TaskpoolScheduler)
					.Where(_ => _streamingStarted && IsConnected)
					.Select(_ => new RequestRecentMessagesMessage(_room.Id))));
			Subscribe(
				() => _bus.Listen<MessagesReceivedMessage>().Where(msg => msg.RoomId == room.Id).SubscribeUI(HandleMessagesReceived));
			Subscribe(
				() =>
				_bus.Listen<RoomInfoReceivedMessage>().Where(msg => msg.Room.Id == _room.Id).SubscribeUI(HandleRoomInfoReceived));
			Subscribe(() => _bus.Listen<UsersUpdatedMessage>().SubscribeUI(HandleUsersUpdated));

			_bus.SendMessage(new RequestRecentMessagesMessage(room.Id));
			_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));

			Subscribe(() => _bus.RegisterMessageSource(Observable.Interval(TimeSpan.FromMinutes(5)).Select(
				_ => new RequestKeepAliveMessage(_room.Id))));

			PasteCommand = new ReactiveCommand();

			Subscribe(() => PasteCommand.Select(pc => clipboardService.GetFileItem())
								.Where(ci => ci != null)
			                	.Subscribe(ci => _chatDocument.AddUploadFile(_room, ci)));

			var leaveRoomCommand = commands.LeaveRoomCommand.OfType<int>().Where(id => id == _room.Id);
			Subscribe(_bus.RegisterMessageSource(leaveRoomCommand.Select(_ => new RequestStopStreamingMessage(_room.Id))));
			Subscribe(_bus.RegisterMessageSource(leaveRoomCommand.Select(_ => new RequestLeaveRoomMessage(_room.Id))));
			Subscribe(
				_bus.RegisterMessageSource(
					leaveRoomCommand.Select(_ => new ActivateModuleByIdMessage(ModuleNames.MainCampfireView, ModuleIds.Lobby))));

			UploadFileCommand = new ReactiveCommand();

			Subscribe(_bus.RegisterSourceAndHandleReply<RequestUploadFilePickerMessage, UploadFilePickedMessage>(
				UploadFileCommand.Select(_ => new RequestUploadFilePickerMessage()), 
				response => _chatDocument.AddUploadFile(_room, response.FileItem)));
		}

		public ReactiveCommand UploadFileCommand { get; private set; }


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
			Topic = _room.Topic;
			if (_room.Users != null)
			{
				PopulateUsers(_room.Users);
			}
			RoomTranscriptsUri = String.Format(@"http://{0}.campfirenow.com/files+transcripts?room_id={1}",
				obj.AccountName, obj.Room.Id);
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

				if (message.Type == MessageType.TopicChangeMessage)
				{
					Topic = message.Body;
				}

				_sinceMessageId = message.Id;
			}

			if (newMessages.Any())
			{
				_bus.SendMessage(new NewMessagesReceivedMessage(newMessages, RoomId));
			}

			if (newMessages.Any(msg => msg.Type.In(MessageType.EnterMessage, MessageType.KickMessage, MessageType.LeaveMessage)))
			{
				_bus.SendMessage(new RequestRoomInfoMessage(_room.Id));
			}

			var count = newMessages.Count(msg => 
				msg.Type.In(MessageType.PasteMessage, MessageType.TextMessage, MessageType.UploadMessage));
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

			PurgeOldMessages();
		}

		private void PurgeOldMessages()
		{
			if (_messages.Count < _settings.General.MaxBackLog)
			{
				return;
			}
			var toPurge = _messages.Take(_messages.Count - _settings.General.MaxBackLog).ToArray();

			foreach (var roomMessage in toPurge)
			{
				_chatDocument.RemoveMessage(roomMessage.TextObject);
				_messages.Remove(roomMessage);
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
		public string Caption
		{
			get { return RoomName; }
		}

		public int RoomId
		{
			get { return _room.Id; }
		}

		public string Topic
		{
			get { return _topic; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Topic, ref _topic, value); }
		}

		public bool IsConnected
		{
			get { return _isConnected; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsConnected, ref _isConnected, value); }
		}

		public string RoomTranscriptsUri
		{
			get { return _roomTranscriptsUri; }
			private set { this.RaiseAndSetIfChanged(vm => vm.RoomTranscriptsUri, ref _roomTranscriptsUri, value); }
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
				UserEditedMessage = !String.IsNullOrEmpty(value);
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
			get { return _notificationCount > 0 ? _notificationCount.ToString(CultureInfo.InvariantCulture) : ""; }
		}

		public bool Closable { get { return true; }}

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
}
