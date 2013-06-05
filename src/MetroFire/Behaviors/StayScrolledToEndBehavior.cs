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
		private bool _wasScrolledToEnd;

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			
		}


		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_wasScrolledToEnd = true;

			_scrollViewer = AssociatedObject.FindVisualChild<ScrollViewer>();

			_scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;

		}

		private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.ExtentHeightChange > 0)
			{
				if (_wasScrolledToEnd)
				{
					_scrollViewer.ScrollToEnd();
				}
			}
			else if (Math.Abs(e.VerticalChange) > 0)
			{
				var bottomOffset = _scrollViewer.VerticalOffset + _scrollViewer.ViewportHeight;
				var totalHeight = _scrollViewer.ExtentHeight;
				var deltaFromBottom = Math.Abs(bottomOffset - totalHeight);

				// allow for about half a line of stickiness at the bottom
				_wasScrolledToEnd = deltaFromBottom < (Math.Abs(e.VerticalChange) /2);
			}
		}
	}
}
