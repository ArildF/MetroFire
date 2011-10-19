using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class Clipboard : IClipboard
	{
		public string GetText()
		{
			return System.Windows.Clipboard.ContainsText() ? System.Windows.Clipboard.GetText() : null;
		}

		public BitmapSource GetImage()
		{
			return System.Windows.Clipboard.ContainsImage() ? System.Windows.Clipboard.GetImage() : null;
		}
	}
}
