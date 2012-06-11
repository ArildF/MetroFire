using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class CustomPasteBehavior : Behavior<TextBox>
	{
		/// <summary>
		/// Description
		/// </summary>
		public static readonly DependencyProperty CommandProperty;

		private CommandBinding _originalBinding;

		static CustomPasteBehavior()
		{
			var md = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None);
			CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomPasteBehavior), md);                                                      
			
		}

		/// <summary>
		/// A property wrapper for the <see cref="CommandProperty"/>
		/// dependency property:<br/>
		/// Description
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			_originalBinding = AssociatedObject.CommandBindings.Cast<CommandBinding>().FirstOrDefault(
				cb => cb.Command == ApplicationCommands.Paste);
			var commandBinding = new CommandBinding(ApplicationCommands.Paste, Executed);
			//commandBinding.PreviewCanExecute += CommandBindingOnPreviewCanExecute;
			//commandBinding.PreviewExecuted += CommandBindingOnPreviewExecuted;
			AssociatedObject.CommandBindings.Add(commandBinding);
		}

		private void CommandBindingOnPreviewExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{
			
		}

		private void CommandBindingOnPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}


		private void Executed(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{
			if (Clipboard.ContainsText())
			{
				AssociatedObject.Paste();
			}
			else
			{
				if (Command != null && Command.CanExecute(executedRoutedEventArgs.Parameter))
				{
					Command.Execute(executedRoutedEventArgs.Parameter);
				}
			}
		}
	}
}
