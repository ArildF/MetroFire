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

			var mdModifier = new FrameworkPropertyMetadata(ModifierKeys.None, FrameworkPropertyMetadataOptions.None);
			ModifierProperty = DependencyProperty.Register("Modifier", typeof(ModifierKeys?), typeof(CustomKeyBindingTrigger),
														   mdModifier);

			var mdSourceObject = new FrameworkPropertyMetadata{PropertyChangedCallback = SourceObjectChanged};
			SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof (UIElement),
				typeof (CustomKeyBindingTrigger), mdSourceObject);
		}

		#region SourceObject dependency property

		/// <summary>
		/// Description
		/// </summary>
		public static readonly DependencyProperty SourceObjectProperty;



		/// <summary>
		/// A property wrapper for the <see cref="SourceObjectProperty"/>
		/// dependency property:<br/>
		/// Description
		/// </summary>
		public UIElement SourceObject
		{
			get { return (UIElement) GetValue(SourceObjectProperty); }
			set { SetValue(SourceObjectProperty, value); }
		}


		/// <summary>
		/// Handles changes on the <see cref="SourceObjectProperty"/> dependency property. As
		/// WPF internally uses the dependency property system and bypasses the
		/// <see cref="SourceObject"/> property wrapper, updates should be handled here.
		/// </summary>
		/// <param name="d">The currently processed owner of the property.</param>
		/// <param name="e">Provides information about the updated property.</param>
		private static void SourceObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (CustomKeyBindingTrigger) d;
			var newValue = e.NewValue as UIElement;
			var oldValue = e.OldValue as UIElement;

			if(oldValue != DependencyProperty.UnsetValue && oldValue != null)
			{
				owner.Unsubscribe(oldValue);
			}
			if(newValue != DependencyProperty.UnsetValue && newValue != null)
			{
				owner.Unsubscribe(owner.AssociatedObject);
				owner.Subscribe(newValue);
			}

		}

		#endregion


		protected override void OnAttached()
		{
			base.OnAttached();

			var sourceObject = SourceObject;
			if (sourceObject == DependencyProperty.UnsetValue || sourceObject == null)
			{
				Subscribe(AssociatedObject);
			}
		}

		private void Subscribe(UIElement uiElement)
		{
			uiElement.AddHandler(UIElement.TextInputEvent,
			                            new TextCompositionEventHandler(AssociatedObjectOnPreviewTextInput), true);
			uiElement.AddHandler(UIElement.PreviewKeyDownEvent, new KeyEventHandler(AssociatedObjectOnPreviewKeyDown), true);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Unsubscribe(AssociatedObject);
		}

		private void Unsubscribe(UIElement uiElement)
		{
			uiElement.PreviewTextInput -= AssociatedObjectOnPreviewTextInput;
			uiElement.PreviewKeyDown -= AssociatedObjectOnPreviewKeyDown;
		}


		private void AssociatedObjectOnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = e.Handled || Handle(e.Key);
		}

		private void AssociatedObjectOnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (Keyboard.FocusedElement is TextBoxBase)
			{
				return;
			}

			e.Handled = e.Handled || Handle(e.Text);
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