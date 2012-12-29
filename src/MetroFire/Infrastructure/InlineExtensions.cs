using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public static class InlineExtensions
	{
		private static readonly Dictionary<Type, Func<Inline, string>> Extractors = new Dictionary<Type, Func<Inline, string>>
			{
				{typeof(Run), i => ((Run)i).Text}
			};

		public static string GetText(this IEnumerable<Inline> inlines)
		{
			var sb = new StringBuilder();

			foreach (var inline in inlines.Flatten())
			{
				var s = inline.GetText();
				if (s != null)
				{
					sb.Append(s);
				}
			}
			return sb.ToString();
		}

		public static string GetText(this Paragraph paragraph)
		{
			return paragraph.Inlines.GetText();
		}

		public static string GetText(this Inline inline)
		{
			Func<Inline, string> func;
			if (Extractors.TryGetValue(inline.GetType(), out func))
			{
				return func(inline);
			}
			return null;
		}

		public static IEnumerable<Inline> Flatten(this IEnumerable<Inline> inlines)
		{
			foreach (var inline in inlines)
			{
				if (inline is Span)
				{
					foreach (var inner in ((Span)inline).Inlines.Flatten())
					{
						yield return inner;
					}
				}
				else
				{
					yield return inline;
				}
			}
		}
	}
}
