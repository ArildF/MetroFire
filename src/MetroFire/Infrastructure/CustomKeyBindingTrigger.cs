using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class CustomKeyBindingTrigger : TriggerBase<UIElement>
	{

		static CustomKeyBindingTrigger()
		{
			//register dependency property
			var mdText = new FrameworkPropertyMetadata("", TextPropertyChanged);
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CustomKeyBindingTrigger), mdText);

			var mdKey = new FrameworkPropertyMetadata(Key.None, KeyPropertyChanged);
			KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(CustomKeyBindingTrigger), mdKey);

			var mdModifier = new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None);
			ModifierProperty = DependencyProperty.Register("Modifier", typeof(ModifierKeys?), typeof(CustomKeyBindingTrigger),
														   mdModifier);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewTextInput += AssociatedObjectOnPreviewTextInput;
			AssociatedObject.PreviewKeyDown += AssociatedObjectOnPreviewKeyDown;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.PreviewTextInput -= AssociatedObjectOnPreviewTextInput;
			AssociatedObject.PreviewKeyDown -= AssociatedObjectOnPreviewKeyDown;
		}


		private void AssociatedObjectOnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.FocusedElement is TextBoxBase)
			{
				e.Handled = false;
				return;
			}

			e.Handled = Handle(e.Key);
		}

		private void AssociatedObjectOnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = Handle(e.Text);
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

		public static readonly DependencyProperty KeyProperty;

		public static DependencyProperty ModifierProperty;


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
			InvokeActions(null);
			return true;
		}
	}
}