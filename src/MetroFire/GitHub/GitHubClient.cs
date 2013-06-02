using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.GitHub
{
	public class GitHubClient : IGitHubClient
	{
		private const string GitHubUrl = "https://api.github.com/repos/ArildF/metrofire/commits";
		private readonly ISettings _settings;

		public GitHubClient(ISettings settings)
		{
			_settings = settings;
		}

		public Commit[] GetLatestCommits()
		{
			var webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.UserAgent] = CampfireApi.UserAgent;

			if (!_settings.Network.UseProxy)
			{
				webClient.Proxy = null;
			}

			var str = webClient.DownloadString(GitHubUrl);
			var serializer = new JsonSerializer();
			return serializer.Deserialize<Commit[]>(new JsonTextReader(new StringReader(str)));
		}
	}
}
