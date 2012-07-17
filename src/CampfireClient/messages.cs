using System;
using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public class RequestLoginMessage : CorrelatedMessage
	{
		public LoginInfo LoginInfo { get; private set; }

		public RequestLoginMessage(LoginInfo loginInfo)
		{
			LoginInfo = loginInfo;
		}
	}

	public class RequestLoginResponse : CorrelatedReply
	{
		public bool SuccessFul { get; set; }

		public RequestLoginResponse(bool successFul, Guid correlationId) : base(correlationId)
		{
			SuccessFul = successFul;
		}
	}

	public class AccountUpdated
	{
		public IAccount Account { get; private set; }

		public AccountUpdated(IAccount account)
		{
			Account = account;
		}
	}

	public class RoomListMessage
	{
		public Room[] Rooms { get; private set; }

		public RoomListMessage(Room[] rooms)
		{
			Rooms = rooms;
		}
	}

	public class RequestRoomListMessage
	{
	}

	public class CurrentUserInformationReceivedMessage
	{
		public User User { get; private set; }

		public CurrentUserInformationReceivedMessage(User user)
		{
			User = user;
		}
	}

	public class RequestRoomPresenceMessage{}

	public class RoomPresenceMessage
	{
		private readonly Room[] _rooms;

		public RoomPresenceMessage(Room[] rooms)
		{
			_rooms = rooms;
		}

		public Room[] Rooms
		{
			get { return _rooms; }
		}

		public bool IsPresentIn(int id)
		{
			return _rooms.Any(r => r.Id == id);
		}

		public bool IsPresentIn(string caption)
		{
			return _rooms.Any(r => r.Name.Equals(caption, StringComparison.InvariantCultureIgnoreCase));
		}
	}

	public class UserJoinedRoomMessage
	{
		public int Id { get; private set; }

		public UserJoinedRoomMessage(int id)
		{
			Id = id;
		}
	}

	public class RequestJoinRoomMessage
	{
		public int Id { get; private set; }

		public RequestJoinRoomMessage(int id)
		{
			Id = id;
		}
	}
	public class RequestLeaveRoomMessage
	{
		public int Id { get; private set; }

		public RequestLeaveRoomMessage(int id)
		{
			Id = id;
		}
	}

	public class RequestSpeakInRoomMessage
	{
		public int Id { get; private set; }

		public string Message { get; private set; }

		public RequestSpeakInRoomMessage(int id, string message)
		{
			Id = id;
			Message = message;
		}
	}

	public class RequestRecentMessagesMessage
	{
		public int RoomId { get; private set; }
		public int? SinceMessageId { get; private set; }

		public RequestRecentMessagesMessage(int roomId, int? sinceMessageId = null)
		{
			RoomId = roomId;
			SinceMessageId = sinceMessageId;
		}
	}


	public class MessagesReceivedMessage
	{
		public int RoomId { get; private set; }
		public Message[] Messages { get; private set; }

		public int? SinceMessageId { get; private set; }

		public MessagesReceivedMessage(int roomId, Message[] messages, int? sinceMessageId)
		{
			RoomId = roomId;
			Messages = messages;
			SinceMessageId = sinceMessageId;
		}
	}

	public class RequestRoomInfoMessage
	{
		public int Id { get; private set; }

		public RequestRoomInfoMessage(int id)
		{
			Id = id;
		}
	}

	public class RoomInfoReceivedMessage
	{
		public Room Room { get; private set; }

		public RoomInfoReceivedMessage(Room room)
		{
			Room = room;
		}
	}

	public class RequestUserInfoMessage
	{
		public IEnumerable<int> UserIds { get; private set; }

		public RequestUserInfoMessage(IEnumerable<int> ids)
		{
			UserIds = ids;
		}

		public RequestUserInfoMessage(params int[] ids)
		{
			UserIds = ids;
		}
	}

	public class RequestStartStreamingMessage
	{
		public int RoomId { get; private set; }

		public RequestStartStreamingMessage(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class RequestStopStreamingMessage
	{
		public int RoomId { get; private set; }

		public RequestStopStreamingMessage(int roomId)
		{
			RoomId = roomId;
		}
	}


	public class UserInfoReceivedMessage
	{
		public User User { get; private set; }

		public UserInfoReceivedMessage(User user)
		{
			User = user;
		}
	}

	public class ExceptionMessage
	{
		public Exception Exception { get; set; }

		public ExceptionMessage(Exception exception)
		{
			Exception = exception;
		}
	}

	public class CorrelatedExceptionMessage : ExceptionMessage
	{
		public Guid CorrelationId { get; private set; }

		public CorrelatedExceptionMessage(Exception exception, Guid correlationId) : base(exception)
		{
			CorrelationId = correlationId;
		}
	}

	public enum LogMessageType
	{
		Info,
		Warning,
		Debug,
		Error
	}

	public class LogMessage
	{
		public LogMessageType LogMessageType { get; private set; }
		public string Text { get; private set; }

		public LogMessage(LogMessageType logMessageType, string text, params object[] args)
		{
			LogMessageType = logMessageType;
			Text = String.Format(text, args);
		}
	}

	public class RequestKeepAliveMessage
	{
		public int RoomId { get; private set; }

		public RequestKeepAliveMessage(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class RequestUploadMessage
	{
		public int RoomId { get; set; }
		public int MessageId { get; set; }

		public readonly Guid Correlation = Guid.NewGuid();

		public RequestUploadMessage(int roomId, int messageId)
		{
			RoomId = roomId;
			MessageId = messageId;
		}
	}

	public class UploadReceivedMessage
	{
		public Upload Upload { get; private set; }
		public Guid Correlation { get; set; }

		public UploadReceivedMessage(Upload upload, Guid correlation)
		{
			Upload = upload;
			Correlation = correlation;
		}
	}

	public class RequestDownloadFileMessage
	{
		public string Url { get; private set; }

		public RequestDownloadFileMessage(string url)
		{
			Url = url;
		}
	}

	public class FileDownloadedMessage
	{
		public string Url { get; private set; }
		public string File { get; private set; }

		public FileDownloadedMessage(string url, string file)
		{
			Url = url;
			File = file;
		}
	}

	public class CorrelatedMessage
	{
		public Guid CorrelationId { get; set; }

		public CorrelatedMessage()
		{
			CorrelationId = Guid.NewGuid();
		}
	}

	public class CorrelatedReply
	{
		public CorrelatedReply(Guid correlationId)
		{
			CorrelationId = correlationId;
		}

		public Guid CorrelationId { get; private set; }
	}

	public class RequestUploadFileMessage : CorrelatedMessage
	{
		public string Path { get; private set; }
		public string ContentType { get; private set; }
		public int RoomId { get; private set; }

		public RequestUploadFileMessage(int roomId, string path, string contentType)
		{
			RoomId = roomId;
			Path = path;
			ContentType = contentType;
		}
	}

	public class FileUploadedMessage : CorrelatedReply
	{
		public string Path { get; private set; }
		public Upload Upload { get; private set; }

		public FileUploadedMessage(Guid correlationId, string path, Upload upload) : base(correlationId)
		{
			Path = path;
			Upload = upload;
		}
	}

	public class OperationFailedMessage
	{
		public Exception Exception { get; private set; }
		public Guid CorrelationId { get; private set; }

		public OperationFailedMessage(Exception exception, Guid correlationId)
		{
			Exception = exception;
			CorrelationId = correlationId;
		}
	}

	public class FileUploadProgressChangedMessage : CorrelatedReply
	{
		public ProgressState Progress { get; private set; }

		public FileUploadProgressChangedMessage(Guid correlationId, ProgressState progress) : base(correlationId)
		{
			Progress = progress;
		}
	}

	public class ProgressState
	{
		public long Total { get; private set; }
		public long Current { get; private set; }

		public ProgressState(long total, long current)
		{
			Total = total;
			Current = current;
		}
	}

	public class RequestConnectivityCheckMessage{}

	public class RequestCheckAccountName : CorrelatedMessage
	{
		public string AccountName { get; private set; }

		public RequestCheckAccountName(string accountName)
		{
			AccountName = accountName;
		}
	}

	public class RequestCheckAccountNameReply : CorrelatedReply
	{
		public bool Result { get; private set; }

		public RequestCheckAccountNameReply(bool result, Guid correlationId) : base(correlationId)
		{
			Result = result;
		}
	}
}
