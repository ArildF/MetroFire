using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Rogue.MetroFire.UI.Views;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class UrlifyBehavior : Behavior<TextBlock>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			new HyperLinkVisitor().Visit(AssociatedObject.Inlines);
		}
	}
}
