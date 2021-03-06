﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Linq;

namespace MetroFire.Specs.Steps
{
	public class CampfireApiFake : ICampfireApi
	{
		private readonly List<Room> _rooms = new List<Room>();
		private readonly List<Room> _allRooms = new List<Room>();
		private readonly List<Room> _joinedRooms = new List<Room>();
		private readonly List<Message> _messages = new List<Message>(); 
		private readonly List<Streamer>  _streamers = new List<Streamer>();

		private readonly List<User>  _users = new List<User>{new User{Name = "Default", Id=0}};

		private int _currentMessageId;
		private readonly User _meUser;
		private readonly List<string> _validAccounts = new List<string>();
		private bool _throwOnValidatingAccount;
		private LoginInfo _loginInfo;

		public CampfireApiFake()
		{
			_meUser = new User() {Id = 42, Name = "Me"};
			_users.Add(_meUser);
		}

		public Account GetAccountInfo()
		{
			if (_loginInfo != null && _loginInfo.Token != CorrectToken)
			{
				return null;
			}

			return new Account();
		}

		public Room[] ListRooms()
		{
			return _rooms.ToArray();
		}

		public void SetLoginInfo(LoginInfo loginInfo)
		{
			_loginInfo = loginInfo;

		}

		public Room[] ListPresence()
		{
			return JoinedRooms.ToArray();
		}

		public Unit Join(int id)
		{
			JoinedRooms.Add(_rooms.First(r => r.Id == id));
			return Unit.Default;
		}

		public Message Speak(int id, string text)
		{
			NewRoomMessage(text, _rooms.Single(r => r.Id == id).Name, _meUser.Id);

			return _messages.Last();
		}

		public Message[] GetMessages(int id, int? sinceId = new int?())
		{
			return _messages.Where(m => m.RoomId == id).ToArray();
		}

		public Room GetRoom(int id)
		{
			return _rooms.Single(r => r.Id == id);
		}

		public User GetUser(int id)
		{
			return _users.Single(u => u.Id == id);
		}

		public IEnumerable<User> Users()
		{
			return _users;
		}

		public IDisposable Stream(int id, Action<Message> action, IObserver<ConnectionState> observer)
		{
			var streamer = new Streamer(id, action, observer);
			_streamers.Add(streamer);

			return ActionDisposable.Create(() => _streamers.Remove(streamer));
		}

		public Upload GetUpload(int roomId, int uploadMessageId)
		{
			throw new NotImplementedException();
		}

		public Unit DownloadFile(string uri, string destination)
		{
			throw new NotImplementedException();
		}

		public Upload UploadFile(int roomId, UploadFileParams uploadFileParams, IObserver<ProgressState> progressObserver)
		{
			throw new NotImplementedException();
		}

		public User AddUser(string name)
		{
			var user = new User {Name = name, Id = _users.Max(u => u.Id) + 1};
			_users.Add(user);
			return user;
		}

		public Unit LeaveRoom(int id)
		{
			JoinedRooms.RemoveAll(r => r.Id == id);

			return Unit.Default;
		}

		public User GetMe()
		{
			return _meUser;
		}

		public string CorrectToken { get; set; }

		public List<Room> JoinedRooms
		{
			get { return _joinedRooms; }
		}

		public ConnectivityState CheckConnectivity()
		{
			throw new NotImplementedException();
		}

		public bool CheckAccountExists(string account)
		{
			if (_throwOnValidatingAccount)
			{
				throw new WebException("Ohai", WebExceptionStatus.NameResolutionFailure);
			}
			return _validAccounts.Contains(account);
		}


		public HeadInfo Head(string url)
		{
			throw new NotImplementedException();
		}

		public string AccountName { get; private set; }

		public void AddRoom(Room room)
		{
			if (room.Users == null)
			{
				room.Users = new User[]{};
			}
			_rooms.Add(room);
			_allRooms.Add(room);
		}

		public int IdForRoom(string roomName)
		{
			return _allRooms.First(r => r.Name == roomName).Id;
		}

		public class Streamer
		{
			public int Id { get; private set; }
			public Action<Message> Action { get; private set; }
			public IObserver<ConnectionState> Observer { get; private set; }

			public Streamer(int id, Action<Message> action, IObserver<ConnectionState> observer)
			{
				Id = id;
				Action = action;
				Observer = observer;
			}
		}

		private class ActionDisposable : IDisposable
		{
			private readonly Action _action;

			private ActionDisposable(Action action)
			{
				_action = action;
			}

			public static IDisposable Create(Action action)
			{
				return new ActionDisposable(action);
			}

			public void Dispose()
			{
				_action();
			}
		}


		public void NewRoomMessage(string messageText, string roomName, int userId = 0)
		{
			int id = IdForRoom(roomName);
			var message = new Message
				{
					Body = messageText,
					CreatedAt = DateTime.Now,
					Id = _currentMessageId++,
					MessageTypeString = MessageType.TextMessage.ToString(),
					RoomId = id,
					UserIdString = userId.ToString()
				};

			AddMessage(message);
		}

		private void AddMessage(Message message)
		{
			_messages.Add(message);
			var streamer = _streamers.FirstOrDefault(s => s.Id == message.RoomId);
			if (streamer != null)
			{
				streamer.Action(message);
			}
		}

		public void SendRoomMessages(string roomName, Message[] messages)
		{
			var id = IdForRoom(roomName);
			foreach (var message in messages)
			{
				message.RoomId = id;
				AddMessage(message);
			}
		}

		public void SetStreamingConnectionState(string roomName, bool connected)
		{
			var id = IdForRoom(roomName);
			var streamer = _streamers.Single(s => s.Id == id);
			streamer.Observer.OnNext(new ConnectionState(id, connected));
		}

		public IEnumerable<Streamer> Streamers()
		{
			return _streamers;
		}

		public void IsInvalidAccount(string accountName)
		{
			
		}

		public void IsValidAccount(string accountName)
		{
			_validAccounts.Add(accountName);
		}

		public void ThrowOnValidatingAccount()
		{
			_throwOnValidatingAccount = true;
		}

		public void DontThrowOnValidatingAccount()
		{
			_throwOnValidatingAccount = false; 
		}
	}
}
