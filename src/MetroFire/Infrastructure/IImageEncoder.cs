using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ImageEncoder : IImageEncoder
	{
		public string EncodeToTempPng(BitmapSource bitmapSource)
		{
			var path = Path.GetTempFileName();
			using (var fs = File.OpenWrite(path))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
				encoder.Save(fs);
			}

			return path;
		}
	}

	
}
