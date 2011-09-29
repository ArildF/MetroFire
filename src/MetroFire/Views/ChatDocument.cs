using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.Views
{
	public class ChatDocument : FlowDocument, IChatDocument
	{
		private readonly Dictionary<MessageType, Action<Message, User, Paragraph>> _handlers;
			

		public ChatDocument()
		{
			_handlers = new Dictionary<MessageType, Action<Message, User, Paragraph>>
				{
					{MessageType.TextMessage, FormatUserMessage},
					{MessageType.TimestampMessage, FormatTimestampMessage},
					{MessageType.LeaveMessage, FormatLeaveMessage},
					{MessageType.KickMessage, FormatKickMessage},
					{MessageType.PasteMessage, FormatPasteMessage},
					{MessageType.EnterMessage, FormatEnterMessage},
					{MessageType.UploadMessage, FormatUploadMessage}

				};
		}

		private void FormatUploadMessage(Message msg, User user, Paragraph paragraph)
		{
			//paragraph.Inlines.Add();
		}

		public object AddMessage(Message message, User user)
		{
			Action<Message, User, Paragraph> handler;
			if (!_handlers.TryGetValue(message.Type, out handler))
			{
				return null;
			}
			var paragraph = new Paragraph {Margin = new Thickness(0)};
			handler(message, user, paragraph);

			Blocks.Add(paragraph);

			return paragraph;
		}

		private void FormatEnterMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} entered the room", name)));
		}

		private void FormatPasteMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(name + ": " + Environment.NewLine);

			var run = new Run(message.Body) {FontFamily = new FontFamily("Consolas"), Background = Brushes.LightGray};
			paragraph.Inlines.Add(run);

			paragraph.BorderThickness = new Thickness(0.5);
			paragraph.BorderBrush = Brushes.Black;
		}

		private void FormatLeaveMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} left the room", name)));
		}

		private void FormatKickMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} was kicked from the room", name)));
		}

		private void FormatUserMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);

			paragraph.Inlines.Add(name + ": ");
			paragraph.Inlines.Add(message.Body);
		}

		private static string FormatUserName(User user)
		{
			var name = user != null ? user.Name : "<unknown>";
			return name;
		}

		private void FormatTimestampMessage(Message message, User user, Paragraph paragraph)
		{
			var run = CreateMetaRun(message.CreatedAt.ToLocalTime().ToString());
			paragraph.Inlines.Add(run);
		}

		private static Run CreateMetaRun(string message)
		{
			return new Run(message) {FontStyle = FontStyles.Italic, Foreground = Brushes.Gray};
		}

		public void UpdateMessage(object textObject, Message message, User user)
		{
			var paragraph = textObject as Paragraph;
			if (paragraph == null)
			{
				throw new InvalidOperationException("Invalid object passed in");
			}

			Action<Message, User, Paragraph> handler;
			if (!_handlers.TryGetValue(message.Type, out handler))
			{
				return;
			}
			paragraph.Inlines.Clear(); 
			handler(message, user, paragraph);
		}
	}
}
