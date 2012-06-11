using System.Windows;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public static class InputBehavior
	{
		public static readonly DependencyProperty CustomKeyBindingsProperty =
		   DependencyProperty.RegisterAttached(
			   "CustomKeyBindings",
			   typeof(CustomKeyBindingsCollection),
			   typeof(InputBehavior),
			   new PropertyMetadata(new CustomKeyBindingsCollection(), CustomKeyBindingsChanged));

		private static void CustomKeyBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as FrameworkElement;
			var coll = e.NewValue as CustomKeyBindingsCollection;
			if (element != null && coll != null)
			{
				coll.Element = element;

				coll.DataContext = element.DataContext;
				element.DataContextChanged += delegate { coll.DataContext = element.DataContext; };

			}


		}


		public static void SetCustomKeyBindings(FrameworkElement obj,
												  CustomKeyBindingsCollection value)
		{
			obj.SetValue(CustomKeyBindingsProperty, value);
		}

		public static CustomKeyBindingsCollection GetCustomKeyBindings(FrameworkElement obj)
		{
			return (CustomKeyBindingsCollection)obj.GetValue(CustomKeyBindingsProperty);
		}
	}
}
