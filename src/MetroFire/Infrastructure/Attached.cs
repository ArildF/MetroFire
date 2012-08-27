using System.Windows;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class Attached
	{
		public static readonly DependencyProperty ImageBrushProperty =
			DependencyProperty.RegisterAttached("ImageBrush", typeof (Brush), typeof (Attached));

		public static void SetImageBrush(DependencyObject obj, Brush elt)
		{
			obj.SetValue(ImageBrushProperty, elt);
		}

		public static Brush GetImageBrush(DependencyObject obj)
		{
			return (Brush) obj.GetValue(ImageBrushProperty);
		}
	}
}
