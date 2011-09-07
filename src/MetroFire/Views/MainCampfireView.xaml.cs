using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for MainCampfireView.xaml
	/// </summary>
	public partial class MainCampfireView : IMainModule
	{
		private readonly IModuleResolver _resolver;

		protected MainCampfireView()
		{
			InitializeComponent();
		}

		public MainCampfireView(IMainCampfireViewModel model, IModuleResolver resolver) : this()
		{
			DataContext = model;
			_resolver = resolver;
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
