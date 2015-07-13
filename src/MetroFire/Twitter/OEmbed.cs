using Newtonsoft.Json;

namespace Rogue.MetroFire.UI.Twitter
{
	public class OEmbed
	{
		[JsonProperty("html")]
		public string Html { get; set; }
	}
}
