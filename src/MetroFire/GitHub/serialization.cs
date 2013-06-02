using System;
using Newtonsoft.Json;

namespace Rogue.MetroFire.UI.GitHub
{
	public class Commit
	{
		[JsonProperty("sha")]
		public string Sha { get; set; }

		[JsonProperty("commit")]
		public CommitDetails CommitDetails { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("html_url")]
		public string HtmlUrl { get; set; }

		[JsonProperty("author")]
		public AuthorDetails Author { get; set; }

	}

	public class CommitDetails
	{
		[JsonProperty("author")]
		public Author Author { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

	}

	public class Author
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("date")]
		public DateTime Date { get; set; }
	}

	public class AuthorDetails
	{
		[JsonProperty("avatar_url")]
		public string AvatarUrl { get; set; }
	}
}
