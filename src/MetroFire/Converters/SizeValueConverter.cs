using System;
using System.Globalization;
using System.Windows.Data;

namespace Rogue.MetroFire.UI.Converters
{
	public class SizeValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var val = (long) value;

			if (val > 1.MegaBytes())
			{
				return val.AsMegaBytes() + "MB";
			}
			return val.AsKiloBytes() + "KB";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
