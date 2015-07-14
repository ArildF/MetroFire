using System;
using System.Reactive;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class BindableFocusBehavior : Behavior<UIElement>
	{
		public static readonly DependencyProperty BindableIsFocusedProperty = DependencyProperty.Register("BindableIsFocused", typeof(bool),
			typeof(BindableFocusBehavior), new PropertyMetadata(PropertyChanged));

		public static readonly DependencyProperty FocusOnObservableProperty;

		private IDisposable _subscription;

		static BindableFocusBehavior()
		{
			FrameworkPropertyMetadata md = new FrameworkPropertyMetadata(null, FocusOnObservablePropertyChanged);
			FocusOnObservableProperty = DependencyProperty.Register("FocusOnObservable", typeof (IObservable<Unit>),
				typeof (BindableFocusBehavior), md);
		}


		public IObservable<Unit> FocusOnObservable
		{
			get { return (IObservable<Unit>) GetValue(FocusOnObservableProperty); }
			set { SetValue(FocusOnObservableProperty, value); }
		}


		private static void FocusOnObservablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			BindableFocusBehavior owner = (BindableFocusBehavior) d;
			IObservable<Unit> newValue = (IObservable<Unit>) e.NewValue;

			owner.Subscribe(newValue);



		}

		private void Subscribe(IObservable<Unit> newValue)
		{
			if (_subscription != null)
			{
				_subscription.Dispose();
			}

			if (newValue != null)
			{
				_subscription = newValue.Subscribe(_ => Keyboard.Focus(AssociatedObject));
			}
		}


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
