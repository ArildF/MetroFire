using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using Rogue.MetroFire.UI.GitHub;

namespace Rogue.MetroFire.UI.Converters
{
	public class GitHubCommitToColorConverter : IValueConverter
	{
		private readonly DateTime _assemblyVersionTimeStamp;

		private static readonly DateTime Epoch = new DateTime(2012,07,17);

		public GitHubCommitToColorConverter()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			var timeSpan = TimeSpan.FromHours(version.Build);
			_assemblyVersionTimeStamp = Epoch + timeSpan;

		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var commit = (Commit) value;

			if (commit.CommitDetails.Author.Date > _assemblyVersionTimeStamp)
			{
				return Brushes.LightGoldenrodYellow;
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
