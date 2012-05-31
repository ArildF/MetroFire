using System.IO;
using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ImageEncoder : IImageEncoder
	{
		public string EncodeToTempPng(BitmapSource imageSource)
		{
			var path = GetTempFileName();
			using (var fs = File.OpenWrite(path))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(imageSource));
				encoder.Save(fs);
			}

			return path;
		}

		private static string GetTempFileName()
		{
			string filename;
			do
			{
				filename = Path.GetTempFileName() + ".png";

			} while (File.Exists(filename));

			return filename;
		}
	}

	
}
