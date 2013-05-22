namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ClipboardService : IClipboardService
	{
		private readonly IClipboard _clipboard;
		private readonly IImageEncoder _encoder;
		private readonly IMimeTypeResolver _mimeTypeResolver;


		public ClipboardService(IClipboard clipboard, IImageEncoder encoder, IMimeTypeResolver mimeTypeResolver)
		{
			_clipboard = clipboard;
			_encoder = encoder;
			_mimeTypeResolver = mimeTypeResolver;
		}

		public FileItem GetFileItem()
		{
			var image = _clipboard.GetImage();
			if (image != null)
			{
				string path = _encoder.EncodeToTempPng(image);

				return new FileItem(path, "image/png");
			}

			string filePath = _clipboard.GetFilePath();
			if (filePath != null)
			{
				string contentType = _mimeTypeResolver.GetMimeType(filePath);
				return new FileItem(filePath, contentType);
			}

			return null;
		}
	}
}
