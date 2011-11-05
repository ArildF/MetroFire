using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using System.Linq;

namespace MetroFire.Specs.Steps
{
	public class ChatViewFake : IChatDocument
	{
		private readonly List<MessageEntry> _messages = new List<MessageEntry>();

		public object AddMessage(Message message, User user)
		{
			var entry = new MessageEntry(message, user);

			_messages.Add(entry);
			return entry;
		}

		public void UpdateMessage(object textObject, Message message, User user)
		{
			throw new System.NotImplementedException();
		}

		public void AddPasteFile(IRoom room, string path)
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
	}
}