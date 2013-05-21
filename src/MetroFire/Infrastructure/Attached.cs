using System.Windows;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class Attached
	{
		public static readonly DependencyProperty ImageBrushProperty =
			DependencyProperty.RegisterAttached("ImageBrush", typeof (Brush), typeof (Attached));

		public static readonly DependencyProperty HtmlDocumentProperty =
			DependencyProperty.RegisterAttached("HtmlDocument", typeof (string),
				typeof (Attached), new PropertyMetadata(HtmlDocumentPropertyChanged));

		private static void HtmlDocumentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var browser = (System.Windows.Controls.WebBrowser) dependencyObject;
			browser.NavigateToString((string) e.NewValue);
		}

		public static void SetHtmlDocument(DependencyObject obj, string document)
		{
			obj.SetValue(HtmlDocumentProperty, document);
		}

		public static object GetHtmlDocument(DependencyObject obj)
		{
			return obj.GetValue(HtmlDocumentProperty);
		}

		public static void SetImageBrush(DependencyObject obj, Brush elt)
		{
			obj.SetValue(ImageBrushProperty, elt);
		}

		public static Brush GetImageBrush(DependencyObject obj)
		{
			return (Brush) obj.GetValue(ImageBrushProperty);
		}
	}
}
