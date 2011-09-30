using System;
using System.Diagnostics;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class WebBrowser : IWebBrowser
	{
		public void NavigateTo(Uri uri)
		{
			Process.Start(uri.ToString());
		}
	}
}
