using System.Collections.Generic;
using System.Windows.Media;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using System.Linq;
using Rogue.MetroFire.UI.Infrastructure;

namespace MetroFire.Specs.Steps
{
	public class ChatViewFake : IChatDocument
	{
		private readonly List<MessageEntry> _messages = new List<MessageEntry>();

		public object AddMessage(Message message, User user, IRoom room, object textObject)
		{
			var entry = new MessageEntry(message, user);

			var afterEntry = textObject as MessageEntry;
			if (afterEntry != null)
			{
				var index = _messages.IndexOf(afterEntry);
				_messages.Insert(index, entry);
			}
			else
			{
				_messages.Add(entry);
			}
			return entry;
		}

		public void UpdateMessage(object textObject, Message message, User user, IRoom room)
		{
			throw new System.NotImplementedException();
		}

		public void AddUploadFile(IRoom room, FileItem fileItem)
		{
			throw new System.NotImplementedException();
		}

		public double FontSize
		{
			set ;
			get ;
		}

		public void RemoveMessage(object textObject)
		{
		}

		public void AddNotificationMessage(string message, bool isError)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<Message> Messages
		{
			get { return _messages.Select(m => m.Message); }
		}

		private class MessageEntry
		{
			public MessageEntry(Message message, User user)
			{
				Message = message;
				User = user;
			}

			public User User { get; private set; }

			public Message Message { get; private set; }
		}

		public void HighlightMessage(Message message, Color color)
		{
			throw new System.NotImplementedException();
		}
	}
}