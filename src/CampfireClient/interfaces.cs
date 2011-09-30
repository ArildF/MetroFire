using System;
using System.Collections.Generic;
using System.Reactive;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public interface ICampfire
	{
		IAccount Account { get; }
		IEnumerable<Room> Rooms { get; }
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
		IDisposable Stream(int id, Action<Message> action, Action<Exception> onError = null);
		Upload GetUpload(int roomId, int uploadMessageId);
		Unit DownloadFile(string uri, string destination);
	}


}
