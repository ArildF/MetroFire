using System;
using System.Windows.Documents;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.Views
{
	public class ChatDocument : FlowDocument, IChatDocument
	{
		public object AddMessage(User user, string type, string body)
		{
			if (type != "TextMessage")
			{
				return null;
			}
			var paragraph = new Paragraph();

			FormatUserMessage(user, body, paragraph);
			Blocks.Add(paragraph);

			return paragraph;
		}

		private void FormatUserMessage(User user, string body, Paragraph paragraph)
		{
			var name = user != null ? user.Name : "<unknown>";

			paragraph.Inlines.Add(name + ": ");
			paragraph.Inlines.Add(body);
		}

		public void UpdateMessage(object textObject, User user, string type, string body)
		{
			var paragraph = textObject as Paragraph;
			if (paragraph == null)
			{
				throw new InvalidOperationException("Invalid object passed in");
			}
			paragraph.Inlines.Clear(); 
			FormatUserMessage(user, body, paragraph);
		}
	}
}
