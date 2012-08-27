using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for NavigationContent.xaml
	/// </summary>
	public partial class NavigationContent : INavigationContent
	{
		private readonly INavigationContentViewModel _vm;

		private NavigationContent()
		{
			InitializeComponent();
		}

		public NavigationContent(INavigationContentViewModel vm) : this()
		{
			_vm = vm;
			DataContext = vm;
		}

		public DependencyObject Visual
		{
			get { return this; }
		}
	}

}
