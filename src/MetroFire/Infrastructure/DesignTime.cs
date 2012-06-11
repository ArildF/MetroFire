using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public static class DesignTime
	{
		private static readonly Brush DefaultBackgroundBrush =
			new SolidColorBrush(Colors.DarkGray);

		public static Brush GetBackground(DependencyObject obj)
		{
			return (Brush) obj.GetValue(BackgroundProperty);
		}

		public static void SetBackground(DependencyObject obj, Brush value)
		{
			obj.SetValue(BackgroundProperty, value);
		}

		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
			"Background",
			typeof (Brush),
			typeof (DesignTime),
			new PropertyMetadata(
				DefaultBackgroundBrush,
				OnBackgroundPropertyChanged));

		private static void OnBackgroundPropertyChanged(
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!DesignerProperties.GetIsInDesignMode(d))
				return;

			var p = d as Panel;
			if (p != null)
				p.Background = e.NewValue as Brush;

			var c = d as Control;
			if (c != null)
			{
				c.Background = e.NewValue as Brush;
			}
		}

	}
}
