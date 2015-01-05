using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using ColorCode;
using ColorCode.Parsing;

namespace Rogue.MetroFire.UI.Infrastructure
{
	class WpfFormatter : ColorCode.IFormatter
	{
		private readonly Span _inline = new Span{FontFamily = new FontFamily("Consolas")};
		public Inline Inline { get { return _inline; } }

		public void Write(string parsedSourceCode, IList<Scope> scopes, IStyleSheet styleSheet, TextWriter textWriter)
		{
			var item = new Run(parsedSourceCode);
			_inline.Inlines.Add(item);
			if (scopes.Any())
			{
				var firstScope = scopes.First();

				// dynamic here to work around a bug in the XAML compiler
				// XAML compilation will fail if I add a reference to System.Drawing
				dynamic style = styleSheet.Styles[firstScope.Name];
				dynamic color = style.Foreground;
				item.Foreground = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
			}
		}

		public void WriteFooter(IStyleSheet styleSheet, TextWriter textWriter)
		{
		}

		public void WriteHeader(IStyleSheet styleSheet, TextWriter textWriter)
		{
		}
	}
}
