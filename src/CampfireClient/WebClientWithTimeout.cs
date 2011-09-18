using System;
using System.Net;

namespace Rogue.MetroFire.CampfireClient
{
	internal class WebClientWithTimeout : WebClient
	{
		public int Timeout { get; set; }

		public WebClientWithTimeout()
		{
			Timeout = (int) TimeSpan.FromSeconds(100).TotalMilliseconds;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address);
			var httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.Timeout = Timeout;
			}

			return request;
		}
	}
}
