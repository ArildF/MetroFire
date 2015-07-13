using System;
using System.Windows;
using System.Windows.Media;
using CefSharp;
using CefSharp.Wpf;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class Attached
	{
		public static readonly DependencyProperty ImageBrushProperty =
			DependencyProperty.RegisterAttached("ImageBrush", typeof (Brush), typeof (Attached));

		public static readonly DependencyProperty HtmlDocumentProperty =
			DependencyProperty.RegisterAttached("HtmlDocument", typeof (string),
				typeof (Attached), new PropertyMetadata(HtmlDocumentPropertyChanged));

		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.RegisterAttached("Source", typeof(string),
				typeof(Attached), new PropertyMetadata(SourcePropertyChanged));

		private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var browser = (System.Windows.Controls.WebBrowser) d;
			browser.Navigate((string)e.NewValue);
		}

		private static void HtmlDocumentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (dependencyObject is ChromiumWebBrowser)
			{
				var browser = (ChromiumWebBrowser) dependencyObject;

				browser.FrameLoadStart += BrowserOnFrameLoadStart;
				browser.FrameLoadEnd += BrowserOnFrameLoadEnd;

				browser.LoadHtml((string)e.NewValue, "https://twitter.com/");

			}
			else
			{
				var browser = (System.Windows.Controls.WebBrowser) dependencyObject;
				browser.NavigateToString((string)e.NewValue);
			}
		}

		private static void BrowserOnFrameLoadStart(object sender, FrameLoadStartEventArgs frameLoadStartEventArgs)
		{
			
		}

		private static void BrowserOnFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
		{
			
		}

		public static void SetHtmlDocument(DependencyObject obj, string document)
		{
			obj.SetValue(HtmlDocumentProperty, document);
		}

		public static object GetSource(DependencyObject obj)
		{
			return obj.GetValue(SourceProperty);
		}

		public static void SetSource(DependencyObject obj, string source)
		{
			obj.SetValue(SourceProperty, source);
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
