using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for MainCampfireView.xaml
	/// </summary>
	public partial class MainCampfireView : IMainModule
	{
		public MainCampfireView()
		{
			InitializeComponent();
		}

		public string Caption
		{
			get { return ""; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

	}
}
