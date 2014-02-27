using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Converters
{
	class ColorToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null ? new SolidColorBrush((Color) value) : null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
