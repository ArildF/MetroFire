using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var b = (bool) value;
			return b ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (Visibility) value;
			return v == Visibility.Visible;
		}
	}
}
