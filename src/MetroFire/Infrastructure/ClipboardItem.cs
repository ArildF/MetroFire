namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ClipboardItem
	{
		public ClipboardItem(string localPath, string contentType)
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
	}
}