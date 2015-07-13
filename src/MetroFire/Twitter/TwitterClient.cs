using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Twitter
{
	public class TwitterClient : ITwitterClient
	{
		private readonly ISettings _settings;

		private const string TwitterOEmbedUrl =
			"https://api.twitter.com/1/statuses/oembed.json?id={0}";

		public TwitterClient(ISettings settings)
		{
			_settings = settings;
		}

		public OEmbed GetTweet(string id)
		{
			var webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.UserAgent] = CampfireApi.UserAgent;

			if (!_settings.Network.UseProxy)
			{
				webClient.Proxy = null;
			}

			var url = String.Format(TwitterOEmbedUrl, id);
			var str = webClient.DownloadString(url);
			var serializer = new JsonSerializer();
			var tweet = serializer.Deserialize<OEmbed>(new JsonTextReader(new StringReader(str)));
			//tweet.Html += "<script>" + webClient.DownloadString("https://platform.twitter.com/widgets.js")
			//    + "</script>";

			return tweet;
		}
	}

	
}
