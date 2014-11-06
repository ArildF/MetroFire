using System;
using System.ComponentModel;
using System.Diagnostics;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class WebBrowser : IWebBrowser
	{
		private readonly IMessageBus _bus;

		public WebBrowser(IMessageBus bus)
		{
			_bus = bus;
		}

		public void NavigateTo(Uri uri)
		{
			try
			{
				Process.Start(uri.ToString());
			}
			catch (Win32Exception ex)
			{
				if (ex.ErrorCode == -2147467259)
				{
					_bus.SendMessage(
						new SystemNotificationMessage("No default browser set in your operating system", true));
				}
				else
				{
					throw;
				}
			}
			catch (Exception ex)
			{
				_bus.SendMessage(new ExceptionMessage(ex));
			}
		}
	}
}
