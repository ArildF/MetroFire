namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ClipboardItem
	{
		public ClipboardItem(bool isImage, string localPath, string contentType)
		{
			IsImage = isImage;
			LocalPath = localPath;
			ContentType = contentType;
		}

		public bool IsImage { get; private set; }

		public string LocalPath { get; private set; }

		public string ContentType { get; private set; }
	}
}