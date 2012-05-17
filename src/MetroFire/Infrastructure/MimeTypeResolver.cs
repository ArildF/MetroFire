using System.IO;
using Microsoft.Win32;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class MimeTypeResolver : IMimeTypeResolver
	{
		public string GetMimeType(string path)
		{
			const string mimetype = "application/octet-stream";
			string extension = Path.GetExtension(path.ToLower());
			if (extension == null)
			{
				return null;
			}

			using (var regKey = Registry.ClassesRoot.OpenSubKey(extension))
			{
				if (regKey == null)
				{
					return mimetype;
				}

				var value = regKey.GetValue("Content Type") as string;
				return value ?? mimetype;
			}
		}
	}
}