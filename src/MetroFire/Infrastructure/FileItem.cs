using System.IO;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class FileItem
	{
		public FileItem(string localPath, string contentType)
		{
			LocalPath = localPath;
			ContentType = contentType;
		}

		public bool IsImage
		{
			get { return ContentType.ToLower().StartsWith("image/"); }
		}

		public string LocalPath { get; private set; }

		public string ContentType { get; private set; }

		public long Size
		{
			get
			{
				if (File.Exists(LocalPath))
				{
					return new FileInfo(LocalPath).Length;
				}
				return -1;
			}
		}
	}
}