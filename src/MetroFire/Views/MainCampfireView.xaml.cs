using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Rogue.MetroFire.UI.Transitions;
using Transitionals.Controls;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for MainCampfireView.xaml
	/// </summary>
	public partial class MainCampfireView 
	{
		public MainCampfireView()
		{
			InitializeComponent();
		}


		private void TransitionElementOnLoaded(object sender, RoutedEventArgs e)
		{
			var te = (TransitionElement) sender;
			var tc = GetParent<TabControl>(te);
			if (tc != null)
			{
				((HorizontalSlideTransitionSelector) te.TransitionSelector).Collection = tc.ItemsSource;
			}

		}

		private T GetParent<T>(FrameworkElement te) where T : FrameworkElement
		{
			var parent = VisualTreeHelper.GetParent(te) as FrameworkElement;
			if (parent == null || parent is T)
			{
				return (T)parent;
			}
			return GetParent<T>(parent);

		}
	}
}
