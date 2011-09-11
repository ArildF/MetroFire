using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class BindableFocusBehavior : Behavior<UIElement>
	{
		public static readonly DependencyProperty BindableIsFocusedProperty = DependencyProperty.Register("BindableIsFocused", typeof(bool),
			typeof(BindableFocusBehavior), new PropertyMetadata(PropertyChanged));

		public bool BindableIsFocused
		{
			get { return (bool)GetValue(BindableIsFocusedProperty); }
			set { SetValue(BindableIsFocusedProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.GotFocus += AssociatedObjectOnFocusChanged;
			AssociatedObject.LostFocus += AssociatedObjectOnFocusChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.GotFocus -= AssociatedObjectOnFocusChanged;
			AssociatedObject.LostFocus -= AssociatedObjectOnFocusChanged;
		}

		private void AssociatedObjectOnFocusChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			BindableIsFocused = AssociatedObject.IsFocused;
		}

		private static void PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

		}
	}
}
