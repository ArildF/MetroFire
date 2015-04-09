using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class DoubleClickToCommandBehavior : Behavior<UIElement>
	{

		public static readonly DependencyProperty CommandProperty;


		static DoubleClickToCommandBehavior()
		{
			FrameworkPropertyMetadata md = new FrameworkPropertyMetadata(null, CommandPropertyChanged);
			CommandProperty = DependencyProperty.Register("Command", typeof (ICommand), typeof (DoubleClickToCommandBehavior), md);



			md = new FrameworkPropertyMetadata(null, CommandParameterPropertyChanged);
			CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof (object),
				typeof (DoubleClickToCommandBehavior), md);

		}

		public static readonly DependencyProperty CommandParameterProperty;


		public object CommandParameter
		{
			get { return (object) GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}


		private static void CommandParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DoubleClickToCommandBehavior owner = (DoubleClickToCommandBehavior) d;
			object newValue = (object) e.NewValue;
		}


		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}


		private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DoubleClickToCommandBehavior owner = (DoubleClickToCommandBehavior) d;
			ICommand newValue = (ICommand) e.NewValue;
		}



		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.MouseLeftButtonDown += AssociatedObjectOnMouseDoubleClick;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.MouseLeftButtonDown -= AssociatedObjectOnMouseDoubleClick;
		}

		private void AssociatedObjectOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (mouseButtonEventArgs.ClickCount < 2)
			{
				return;
			}

			if (Command != null && Command.CanExecute(CommandParameter))
			{
				Command.Execute(CommandParameter);
			}
		}
	}
}
