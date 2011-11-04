using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public interface ICampfire
	{
		IAccount Account { get; }
		IEnumerable<Room> Rooms { get; }
	}

	public class UploadFileParams
	{
		private Stream _stream;
		private string _filename;
		private string _contentType;

		public UploadFileParams(Stream stream, string filename, string contentType)
		{
			_stream = stream;
			_filename = filename;
			_contentType = contentType;
		}

		public Stream Stream
		{
			get { return _stream; }
		}

		public string Filename
		{
			get { return _filename; }
		}

		public string ContentType
		{
			get { return _contentType; }
		}
	}

	public interface ICampfireApi
	{
		Account GetAccountInfo();
		Room[] ListRooms();
		void SetLoginInfo(LoginInfo loginInfo);
		Room[] ListPresence();
		Unit Join(int id);
		Message Speak(int id, string text);
		Message[] GetMessages(int id, int? sinceId = null);
		Room GetRoom(int id);
		User GetUser(int id);
		IDisposable Stream(int id, Action<Message> action, IObserver<ConnectionState> observer);
		Upload GetUpload(int roomId, int uploadMessageId);
		Unit DownloadFile(string uri, string destination);
		Upload UploadFile(int roomId, UploadFileParams uploadFileParams, IObserver<ProgressState> progressObserver);
	}

	public interface ISettings
	{
		INetworkSettings Network { get; }
	}

	public interface INetworkSettings
	{
		bool UseProxy { get; }
	}


}
