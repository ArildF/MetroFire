using System.Windows.Documents;

namespace Rogue.MetroFire.UI.Views
{
	public class ChatDocument : FlowDocument, IChatDocument
	{
		public void AddMessage(string type, string body)
		{
			if (type != "TextMessage")
			{
				return;
			}
			var paragraph = new Paragraph(
				new Run(body)
				);

			Blocks.Add(paragraph);
		}
	}
}
