using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class SelectionBehavior : Behavior<TextBox>
	{
		public static readonly DependencyProperty BindableSelectionStartProperty;

		static SelectionBehavior()
		{
			FrameworkPropertyMetadata md = new FrameworkPropertyMetadata(0, BindableSelectionStartPropertyChanged);
			BindableSelectionStartProperty = DependencyProperty.Register("BindableSelectionStart", typeof(int), typeof(SelectionBehavior), md);                                                      
		}

		public int BindableSelectionStart
		{
			get { return (int) GetValue(BindableSelectionStartProperty); }
			set { SetValue(BindableSelectionStartProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.SelectionChanged -= AssociatedObjectOnSelectionChanged;
		}

		private void AssociatedObjectOnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			BindableSelectionStart = AssociatedObject.SelectionStart;
		}


		private static void BindableSelectionStartPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SelectionBehavior owner = (SelectionBehavior) d;
			int newValue = (int) e.NewValue;

			owner.AssociatedObject.SelectionStart = newValue;
		}


	}
}
