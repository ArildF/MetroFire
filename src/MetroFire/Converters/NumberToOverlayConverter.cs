using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Converters
{
	public class NumberToOverlayConverter : IValueConverter
	{
		private readonly Dictionary<int, ImageSource> _cache = new Dictionary<int, ImageSource>();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int number = (int)value;
			if (number == 0)
			{
				return null;
			}

			ImageSource source;
			if (!_cache.TryGetValue(number, out source))
			{
				source = Render(culture, number);
				_cache[number] = source;
			}

			return source;

		}

		private static ImageSource Render(CultureInfo culture, int number)
		{
			var formattedText = new FormattedText(number.ToString(culture), culture, FlowDirection.LeftToRight,
				new Typeface("Tahoma"), 8, Brushes.Black) { TextAlignment = TextAlignment.Center };

			var dv = new DrawingVisual();
			using (var rc = dv.RenderOpen())
			{
				rc.DrawRectangle(new SolidColorBrush(Color.FromRgb(218, 108, 0)), new Pen(Brushes.White, 1),
					new Rect(1, 1, 14, 14));
				rc.DrawText(formattedText, new Point(8, 8 - (formattedText.Height / 2)));
			}

			var rtb = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Default);
			rtb.Render(dv);

			return rtb;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
