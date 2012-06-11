using System.Windows;
using System.Windows.Controls;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for SettingsButtonNavigationContent.xaml
	/// </summary>
	public partial class SettingsButtonNavigationContent : INavigationContent
	{
		private readonly ISettingsViewModel _vm;

		private SettingsButtonNavigationContent()
		{
			InitializeComponent();
		}

		public SettingsButtonNavigationContent(ISettingsViewModel vm) : this()
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
