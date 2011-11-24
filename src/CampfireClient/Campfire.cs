using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Castle.Core;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public class Campfire : ICampfire, IStartable
	{
		private readonly IMessageBus _bus;
		private readonly ICampfireApi _api;

		private Account _account;
		private Room[] _rooms;
		private Room[] _presentRooms;

		private Semaphore _apiSemaphore = new Semaphore(4, 4);

		private readonly IDictionary<int, IDisposable> _currentStreamingRooms = new Dictionary<int, IDisposable>();
		


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
			_bus.Listen<RequestLeaveRoomMessage>().SubscribeThreadPool(LeaveRoom);
			_bus.Listen<RequestSpeakInRoomMessage>().SubscribeThreadPool(SpeakInRoom);

			_bus.Listen<RequestRecentMessagesMessage>().SubscribeThreadPool(GetRecentMessages);

			_bus.Listen<RequestRoomInfoMessage>().SubscribeThreadPool(GetRoomInfo);
			_bus.Listen<RequestUserInfoMessage>().SubscribeThreadPool(GetUserInfo);

			_bus.Listen<RequestStartStreamingMessage>().SubscribeThreadPool(StartStreaming);
			_bus.Listen<RequestKeepAliveMessage>().SubscribeThreadPool(KeepAlive);
			_bus.Listen<RequestUploadMessage>().SubscribeThreadPool(RequestUpload);
			_bus.Listen<RequestDownloadFileMessage>().SubscribeThreadPool(RequestDownloadFile);
			_bus.Listen<RequestUploadFileMessage>().SubscribeThreadPool(RequestUploadFile);
			_bus.Listen<RequestStopStreamingMessage>().SubscribeThreadPool(RequestStopStreaming);
		}

		private void RequestStopStreaming(RequestStopStreamingMessage obj)
		{
			lock (_currentStreamingRooms)
			{
				IDisposable disposable;
				if (_currentStreamingRooms.TryGetValue(obj.RoomId, out disposable))
				{
					disposable.Dispose();
				}
			}
		}

		private void TestRequestUploadFile(RequestUploadFileMessage obj)
		{
			var progressObserver = new Subject<ProgressState>();
			var disposable = _bus.RegisterMessageSource(
				progressObserver.Select(ps => new FileUploadProgressChangedMessage(obj.CorrelationId, ps)));
			var requestStream = new NullStream();

			var buf = new byte[4096];
			var inputStream = File.OpenRead(obj.Path);
			long total = 0;
			long current = 0;
			if (inputStream.CanSeek)
			{
				total = inputStream.Length;
			}

			progressObserver.OnNext(new ProgressState(total, current));

			int read;
			while ((read = inputStream.Read(buf, 0, buf.Length)) > 0)
			{
				requestStream.Write(buf, 0, read);
				current += read;
				progressObserver.OnNext(new ProgressState(total, current));

			}
			Thread.Sleep(1000);
			_bus.SendMessage(new FileUploadedMessage(obj.CorrelationId, obj.Path, new Upload()));
			disposable.Dispose();
		}

		private void RequestUploadFile(RequestUploadFileMessage obj)
		{
			var subject = new Subject<ProgressState>();
			var disposable = _bus.RegisterMessageSource(
				subject.Select(ps => new FileUploadProgressChangedMessage(obj.CorrelationId, ps)));

			CallApi(() => _api.UploadFile(obj.RoomId, 
					new UploadFileParams(File.OpenRead(obj.Path), Path.GetFileName(obj.Path), obj.ContentType),
					subject),
				upload =>
					{
						_bus.SendMessage(new FileUploadedMessage(obj.CorrelationId, obj.Path, upload));
						disposable.Dispose();
					});
		}

		private void RequestDownloadFile(RequestDownloadFileMessage obj)
		{
			var tempPath = Path.GetTempFileName();
			CallApi(() => _api.DownloadFile(obj.Url, tempPath), 
				_ => _bus.SendMessage(new FileDownloadedMessage(obj.Url, tempPath)));
		}

		private void RequestUpload(RequestUploadMessage obj)
		{
			CallApi(() => _api.GetUpload(obj.RoomId, obj.MessageId),
					upload => _bus.SendMessage(new UploadReceivedMessage(upload, obj.Correlation)));
			
		}

		private void LeaveRoom(RequestLeaveRoomMessage requestLeaveRoomMessage)
		{
			CallApi(() => _api.LeaveRoom(requestLeaveRoomMessage.Id), _ => ListRooms(new RequestRoomListMessage()));
		}

		private void KeepAlive(RequestKeepAliveMessage requestKeepAliveMessage)
		{
			Debug.WriteLine("Sending keepalive for room " + requestKeepAliveMessage.RoomId);
			CallApi(() => _api.Join(requestKeepAliveMessage.RoomId), _ => {});
		}

		private void StartStreaming(RequestStartStreamingMessage obj)
		{
			lock (_currentStreamingRooms)
			{
				var subject = new Subject<ConnectionState>();
				_bus.RegisterMessageSource(subject);
				_bus.RegisterMessageSource(
					subject.Where(cs => cs.Exception != null).Select(cs => new ExceptionMessage(cs.Exception)));

				IDisposable d = _api.Stream(obj.RoomId,
							msg => _bus.SendMessage(new MessagesReceivedMessage(obj.RoomId, new[] { msg }, null)),
							subject);
							
				_currentStreamingRooms[obj.RoomId] = d;
			}
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
					CallApi(() => _api.GetMe(), 
						user =>
							{
								_account = account;
								_bus.SendMessage<LoginSuccessfulMessage>(null);
								_bus.SendMessage(new CurrentUserInformationReceivedMessage(user));
							})
				);
		}

		private void CallApi<T>(Func<T> call, Action<T> continuation)
		{
			for (int i = 0; i < 10; i++)
			{
				_apiSemaphore.WaitOne();
				try
				{
					Exception lastException;
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
						                                i));
						Thread.Sleep(TimeSpan.FromSeconds(2));

						if (i == 9)
						{
							_bus.SendMessage(LogMessageType.Error,
											 "Call to Campfire API timed out for the last time. Giving up. Exception follows:");
						}

					}
					catch (WebException ex)
					{
						lastException = ex;
						if (!ex.IsRecoverable())
						{
							break;
						}
					}
					_bus.SendMessage(new ExceptionMessage(lastException));
				}
				finally
				{
					_apiSemaphore.Release();
				}

			}
		}

		public void Start()
		{
			Subscribe();
		}

		public void Stop()
		{
			
		}
	}

	[Serializable]
	public sealed class NullStream : Stream
	{
		internal NullStream() { }

		public override bool CanRead
		{
			[Pure]
			get { return true; }
		}

		public override bool CanWrite
		{
			[Pure]
			get { return true; }
		}

		public override bool CanSeek
		{
			[Pure]
			get { return true; }
		}

		public override long Length
		{
			get { return 0; }
		}

		public override long Position
		{
			get { return 0; }
			set { }
		}

		protected override void Dispose(bool disposing)
		{
			// Do nothing - we don't want NullStream singleton (static) to be closable 
		}

		public override void Flush()
		{
		}


		public override int Read([In, Out] byte[] buffer, int offset, int count)
		{
			return count;
		}

		public override int ReadByte()
		{
			return -1;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			var now = DateTime.Now;
			while ((DateTime.Now - now).TotalMilliseconds < 1) ;
		}

		public override void WriteByte(byte value)
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0;
		}

		public override void SetLength(long length)
		{
		}
	}

}