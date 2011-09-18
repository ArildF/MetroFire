using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class StayScrolledToEndBehavior : Behavior<FlowDocumentScrollViewer>
	{
		private ScrollViewer _scrollViewer;

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			
		}


		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_scrollViewer = AssociatedObject.FindVisualChild<ScrollViewer>();

			_scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
		}

		private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs scrollChangedEventArgs)
		{
			if (scrollChangedEventArgs.ExtentHeightChange > 0)
			{
				_scrollViewer.ScrollToEnd();
			}
		}
	}
}
