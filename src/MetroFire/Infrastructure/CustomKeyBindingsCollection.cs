using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Castle.Core.Internal;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class CustomKeyBindingsCollection : ObservableCollection<CustomKeyBinding>
	{
		private UIElement _element;
		private object _dataContext;

		public UIElement Element
		{
			get
			{
				return _element;
			}
			set
			{
				if (_element != null)
				{
					_element.PreviewTextInput -= ElementPreviewTextInput;
					_element.PreviewKeyDown -= ElementOnPreviewKeyDown;
				}
				_element = value;
				_element.PreviewTextInput += ElementPreviewTextInput;
				_element.PreviewKeyDown += ElementOnPreviewKeyDown;
			}
		}

		private void ElementOnPreviewKeyDown(object sender, KeyEventArgs args)
		{
			HandleKeyDown(args);

		}

		private void HandleKeyDown(KeyEventArgs args)
		{
			args.Handled =
				(from binding in this
				 let handled = binding.Handle(args.Key)
				 where handled
				 select handled).FirstOrDefault();
		}


		void ElementPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (Keyboard.FocusedElement is TextBoxBase)
			{
				e.Handled = false;
				return;
			}

			HandleTextInput(e);
		}

		private void HandleTextInput(TextCompositionEventArgs e)
		{
			e.Handled =
				(from binding in this
				 let handled = binding.Handle(e.Text)
				 where handled
				 select handled).
					FirstOrDefault();
		}

		public object DataContext
		{
			get
			{
				return _dataContext;
			}
			set
			{
				_dataContext = value;
				this.ForEach(binding => binding.DataContext = value);
			}
		}

		public static readonly DependencyProperty CustomKeyBindingsProperty =
		  DependencyProperty.RegisterAttached(
			  "CustomKeyBindings",
			  typeof(CustomKeyBindingsCollection),
			  typeof(CustomKeyBindingsCollection),
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
