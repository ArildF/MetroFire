using System.Windows.Media;
using Rogue.MetroFire.UI.Assets;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class EmojiAsset
	{
		public Emoji Emoji { get; private set; }
		public DrawingBrush Brush { get; private set; }

		public EmojiAsset(Emoji emoji, DrawingBrush brush)
		{
			Emoji = emoji;
			Brush = brush;
		}
	}
}