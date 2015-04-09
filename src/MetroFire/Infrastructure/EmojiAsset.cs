using System.Windows.Media;
using Rogue.MetroFire.UI.Assets;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class EmojiAsset
	{
		public Emoji Emoji { get; private set; }
		public DrawingGroup DrawingGroup { get; private set; }

		public EmojiAsset(Emoji emoji, DrawingGroup drawingGroup)
		{
			Emoji = emoji;
			DrawingGroup = drawingGroup;
		}
	}
}