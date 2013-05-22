using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Converters
{
	public class StringToSchemeColorConverter : IValueConverter
	{
		private static readonly Color[] SchemeColors = new[]
			{
				Color.FromRgb(25, 203, 255),
				Color.FromRgb(0, 110, 141),
				Color.FromRgb(0, 169, 218),
				Color.FromRgb(141, 70, 0),
				Color.FromRgb(218, 108, 0)
			};

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint hashCode = (uint)value.GetHashCode();
			var index = hashCode % SchemeColors.Length;

			return SchemeColors[index];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
