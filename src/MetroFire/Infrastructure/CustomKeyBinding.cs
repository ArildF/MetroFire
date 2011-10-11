using System.Windows;
using System.Windows.Input;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class CustomKeyBinding : FrameworkElement
	{

		static CustomKeyBinding()
		{
			//register dependency property
			var mdText = new FrameworkPropertyMetadata("", TextPropertyChanged);
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CustomKeyBinding), mdText);

			var mdKey = new FrameworkPropertyMetadata(Key.None, KeyPropertyChanged);
			KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(CustomKeyBinding), mdKey);

			var metadata = new FrameworkPropertyMetadata(null, CommandPropertyChanged);
			CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomKeyBinding), metadata);

			var mdModifier = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None);
			ModifierProperty = DependencyProperty.Register("Modifier", typeof(ModifierKeys?), typeof(CustomKeyBinding),
														   mdModifier);
		}

		/// <summary>
		/// Description
		/// </summary>
		public static readonly DependencyProperty TextProperty;


		/// <summary>
		/// A property wrapper for the <see cref="TextProperty"/>
		/// dependency property:<br/>
		/// Description
		/// </summary>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public Key Key
		{
			get { return (Key)GetValue(KeyProperty); }
			set { SetValue(KeyProperty, value); }
		}

		public ModifierKeys? Modifier
		{
			get { return (ModifierKeys?)GetValue(ModifierProperty); }
			set { SetValue(ModifierProperty, value); }
		}

		/// <summary>
		/// Description
		/// </summary>
		public static readonly DependencyProperty CommandProperty;

		public static readonly DependencyProperty KeyProperty;

		public static DependencyProperty ModifierProperty;


		/// <summary>
		/// A property wrapper for the <see cref="CommandProperty"/>
		/// dependency property:<br/>
		/// Description
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}


		/// <summary>
		/// Handles changes on the <see cref="CommandProperty"/> dependency property. As
		/// WPF internally uses the dependency property system and bypasses the
		/// <see cref="Command"/> property wrapper, updates should be handled here.
		/// </summary>
		/// <param name="d">The currently processed owner of the property.</param>
		/// <param name="e">Provides information about the updated property.</param>
		private static void CommandPropertyChanged(DependencyObject d,
											 DependencyPropertyChangedEventArgs
												 e)
		{
		}

		private static void TextPropertyChanged(DependencyObject d,
											 DependencyPropertyChangedEventArgs
												 e)
		{
		}

		private static void KeyPropertyChanged(DependencyObject d,
									 DependencyPropertyChangedEventArgs
										 e)
		{
		}



		public bool Handle(string text)
		{
			return text.Equals(Text) && DoHandle();
		}

		public bool Handle(Key key)
		{
			return key == Key && Modifier == Keyboard.Modifiers && DoHandle();
		}

		private bool DoHandle()
		{
			if (Command == null || !Command.CanExecute(null))
			{
				return false;
			}

			Command.Execute(null);
			return true;
		}
	}
}