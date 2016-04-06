using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rogue.MetroFire.UI.Views
{
	public static class UrlDetector
	{
		private static readonly Regex UrlRegex = new Regex(@"(https?://\S+)");

		public static bool IsMatch(string text)
		{
			var match = UrlRegex.Match(text);
			Uri uri;
			return match != Match.Empty && Uri.TryCreate(match.Value, UriKind.Absolute, out uri);
		}

		public static IEnumerable<string> Split(string text)
		{
			return UrlRegex.Split(text);
		}
	}
}
