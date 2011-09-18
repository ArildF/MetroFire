using System.Collections.Generic;
using System.Net;
using System.Threading;
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
			_bus.Listen<RequestJoinRoomMessage>().SubscribeThreadPool(JoinRoom);
			_bus.Listen<RequestSpeakInRoomMessage>().SubscribeThreadPool(SpeakInRoom);

			_bus.Listen<RequestRecentMessagesMessage>().SubscribeThreadPool(GetRecentMessages);

			_bus.Listen<RequestRoomInfoMessage>().SubscribeThreadPool(GetRoomInfo);
			_bus.Listen<RequestUserInfoMessage>().SubscribeThreadPool(GetUserInfo);
		}

		private void GetUserInfo(RequestUserInfoMessage obj)
		{
			foreach (var userId in obj.UserIds)
			{
				int id = userId;
				CallApi(() =>_api.GetUser(id), 
					user => _bus.SendMessage(new UserInfoReceivedMessage(user)));
			}
		}

		private void GetRoomInfo(RequestRoomInfoMessage requestRoomInfoMessage)
		{
			CallApi(() => _api.GetRoom(requestRoomInfoMessage.Id),
				room => _bus.SendMessage(new RoomInfoReceivedMessage(room)));
		}

		private void GetRecentMessages(RequestRecentMessagesMessage obj)
		{
			CallApi(() => _api.GetMessages(obj.RoomId, obj.SinceMessageId),
				messages => _bus.SendMessage(new MessagesReceivedMessage(obj.RoomId, messages, obj.SinceMessageId)));
		}

		private void SpeakInRoom(RequestSpeakInRoomMessage obj)
		{
			CallApi(() => _api.Speak(obj.Id, obj.Message),
				msg => _bus.SendMessage(new MessagesReceivedMessage(obj.Id, new[] { msg }, null)));
		}

		private void JoinRoom(RequestJoinRoomMessage requestJoinRoomMessage)
		{
			CallApi(() => _api.Join(requestJoinRoomMessage.Id), _ =>
				{
					ListRoomPresence(null);

					_bus.SendMessage(new UserJoinedRoomMessage(requestJoinRoomMessage.Id));
				});
		}

		private void ListRoomPresence(RequestRoomPresenceMessage obj)
		{
			CallApi(() => _api.ListPresence(),
					presentRooms =>
					{
						_presentRooms = presentRooms;
						_bus.SendMessage(new RoomPresenceMessage(_presentRooms));
					});
		}

		private void ListRooms(RequestRoomListMessage obj)
		{
			CallApi(() => _api.ListRooms(), rooms =>
				{
					_rooms = rooms;
					_bus.SendMessage(new RoomListMessage(rooms));
				});
		}

		private void StartLogin(RequestLoginMessage requestLoginMessage)
		{
			_api.SetLoginInfo(requestLoginMessage.LoginInfo);
			CallApi(() => _api.GetAccountInfo(), account =>
				{
					_account = account;
					_bus.SendMessage<LoginSuccessfulMessage>(null);
				});
		}

		private void CallApi<T>(Func<T> call, Action<T> continuation)
		{
			Exception lastException = null;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					var result = call();
					continuation(result);

					return;
				}
				catch (TimeoutException ex)
				{
					lastException = ex;
					_bus.SendMessage(new LogMessage(LogMessageType.Warning, "Call to Campfire API timed out. This is attempt #{0}", 
						i + 1));
					Thread.Sleep(TimeSpan.FromSeconds(10 * (i + 1)));
				}
				catch (WebException ex)
				{
					lastException = ex;
					break;
				}
			}
			_bus.SendMessage(new ExceptionMessage(lastException));
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