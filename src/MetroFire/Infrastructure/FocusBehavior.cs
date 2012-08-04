using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class FocusBehavior : DependencyObject
	{
		/// <summary>
		/// Command Dependency Property.
		/// </summary>
		public static readonly DependencyProperty BindableFocusProperty =
			DependencyProperty.RegisterAttached(
				"BindableFocus",
				typeof(bool),
				typeof(FocusBehavior),
				new PropertyMetadata(BindableFocusChanged));

		public static readonly DependencyProperty FocusFirstElementOnLoadedProperty =
			DependencyProperty.RegisterAttached(
				"FocusFirstElementOnLoaded",
				typeof(bool),
				typeof(FocusBehavior),
				new FrameworkPropertyMetadata(false, FocusFirstElementOnLoadedPropertyChanged));

		public static void SetBindableFocus(UIElement obj,
								bool value)
		{
			HookEvents(obj);
			obj.SetValue(BindableFocusProperty, value);
		}

		public static bool GetBindableFocus(UIElement obj)
		{
			HookEvents(obj);
			return (bool)obj.GetValue(BindableFocusProperty);
		}

		public static void SetFocusFirstElementOnLoaded(DependencyObject obj, bool value)
		{
			obj.SetValue(FocusFirstElementOnLoadedProperty, value);
		}

		public static bool GetFocusFirstElementOnLoaded(DependencyObject obj)
		{
			return (bool)obj.GetValue(FocusFirstElementOnLoadedProperty);
		}

		/// <summary>
		/// Handles changes on the <see cref="FocusFirstElementOnLoadedProperty"/> dependency property. As
		/// WPF internally uses the dependency property system and bypasses the
		/// <see>
		/// 	<cref>FocusFirstElement</cref>
		/// </see> property wrapper, updates should be handled here.
		/// </summary>
		/// <param name="d">The currently processed owner of the property.</param>
		/// <param name="e">Provides information about the updated property.</param>
		private static void FocusFirstElementOnLoadedPropertyChanged(DependencyObject d,
											 DependencyPropertyChangedEventArgs
												 e)
		{
			var newValue = (bool)e.NewValue;

			var element = d as FrameworkElement;

			if (element != null)
			{
				element.Loaded -= FocusFirstElementOnLoaded;
				if (newValue)
				{
					element.Loaded += FocusFirstElementOnLoaded;
				}
			}


		}

		private static void FocusFirstElementOnLoaded(object sender, RoutedEventArgs args)
		{
			var element = sender as UIElement;
			if (element != null)
			{
				element.MoveFocus(
					new TraversalRequest(FocusNavigationDirection.First));
			}
		}


		private static void BindableFocusChanged(DependencyObject d,
									 DependencyPropertyChangedEventArgs e)
		{
			var c = d as UIElement;
			if (c != null && (bool)e.NewValue)
			{
				if (!c.IsEnabled)
				{
					c.IsEnabledChanged += FocusElement;
				}
				else
				{
					var elt = Keyboard.Focus(c);
					bool retVal = c.Focus();
					Trace.WriteLine("Focus move: " + retVal + " Elt: " + elt + " d: " + d);
				}

				HookEvents(c);
			}
		}

		private static void FocusElement(object sender, DependencyPropertyChangedEventArgs e)
		{
			var element = sender as UIElement;
			if (element != null)
			{
				element.IsEnabledChanged -= FocusElement;
				Keyboard.Focus(element);
			}

		}

		private static void HookEvents(UIElement c)
		{
			c.LostKeyboardFocus -= LostKeyboardFocus;
			c.LostKeyboardFocus += LostKeyboardFocus;

			c.GotKeyboardFocus -= GotKeyboardFocus;
			c.GotKeyboardFocus += GotKeyboardFocus;
		}

		static void GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			SetBindableFocus(sender as UIElement, true);
		}

		static void LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			SetBindableFocus(sender as UIElement, false);
		}
	}
}
