using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
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
			return System.Windows.Clipboard.ContainsImage() ? ImageFromClipboardDib() : null;
		}

		public string GetFilePath()
		{
			var paths = System.Windows.Clipboard.GetData("FileNameW") as string[];
			return paths != null && paths.Any() ? paths.First() : null;
		}


		// code based on http://tomlev2.wordpress.com/2009/02/05/wpf-paste-an-image-from-the-clipboard/
		private BitmapSource ImageFromClipboardDib()
		{
			var ms = System.Windows.Clipboard.GetData("DeviceIndependentBitmap") as MemoryStream;
			if (ms != null)
			{
				var dibBuffer = new byte[ms.Length];
				ms.Read(dibBuffer, 0, dibBuffer.Length);

				var infoHeader =
					BinaryStructConverter.FromByteArray<BITMAPINFOHEADER>(dibBuffer);

				int fileHeaderSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER));
				int infoHeaderSize = infoHeader.biSize;
				int fileSize = fileHeaderSize + infoHeader.biSize + infoHeader.biSizeImage;

				var fileHeader = new BITMAPFILEHEADER
					{
						bfType = BITMAPFILEHEADER.BM,
						bfSize = fileSize,
						bfReserved1 = 0,
						bfReserved2 = 0,
						bfOffBits = fileHeaderSize + infoHeaderSize + infoHeader.biClrUsed*4
					};

				byte[] fileHeaderBytes =
					BinaryStructConverter.ToByteArray(fileHeader);

				var msBitmap = new MemoryStream();
				msBitmap.Write(fileHeaderBytes, 0, fileHeaderSize);
				msBitmap.Write(dibBuffer, 0, dibBuffer.Length);
				msBitmap.Seek(0, SeekOrigin.Begin);

				return BitmapFrame.Create(msBitmap);
			}
			return null;
		}

		// ReSharper disable MemberCanBePrivate.Local
		// ReSharper disable InconsistentNaming
		// ReSharper disable FieldCanBeMadeReadOnly.Local
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		private struct BITMAPFILEHEADER
		{
			public static readonly short BM = 0x4d42; // BM

			public short bfType;
			public int bfSize;
			public short bfReserved1;
			public short bfReserved2;
			public int bfOffBits;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFOHEADER
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
// ReSharper restore FieldCanBeMadeReadOnly.Local
// ReSharper restore InconsistentNaming
// ReSharper restore MemberCanBePrivate.Local
		}
	}

	public static class BinaryStructConverter
	{
		public static T FromByteArray<T>(byte[] bytes) where T : struct
		{
			IntPtr ptr = IntPtr.Zero;
			try
			{
				int size = Marshal.SizeOf(typeof(T));
				ptr = Marshal.AllocHGlobal(size);
				Marshal.Copy(bytes, 0, ptr, size);
				object obj = Marshal.PtrToStructure(ptr, typeof(T));
				return (T)obj;
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeHGlobal(ptr);
			}
		}

		public static byte[] ToByteArray<T>(T obj) where T : struct
		{
			IntPtr ptr = IntPtr.Zero;
			try
			{
				int size = Marshal.SizeOf(typeof(T));
				ptr = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(obj, ptr, true);
				var bytes = new byte[size];
				Marshal.Copy(ptr, bytes, 0, size);
				return bytes;
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeHGlobal(ptr);
			}
		}
	}
}
