using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Linq;

namespace Rogue.MetroFire.UI.Behaviors
{
	public class RestrictToNumericalInputBehavior : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.PreviewTextInput += AssociatedObjectOnPreviewTextInput;

		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.PreviewTextInput -= AssociatedObjectOnPreviewTextInput;
		}


		private void AssociatedObjectOnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!e.Text.All(Char.IsDigit))
			{
				e.Handled = true;
			}
		}
	}
}
