using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for SettingsModule.xaml
	/// </summary>
	public partial class SettingsModule : IMainModule
	{
		public SettingsModule()
		{
			InitializeComponent();
		}

		public SettingsModule(ISettingsViewModel vm) : this()
		{
			DataContext = vm;
		}

		public string Caption
		{
			get { return "Settings"; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

		public bool IsActive
		{
			get { return true; }
			set {  }
		}

		public int Id
		{
			get { return -3; }
		}

		public string Notifications
		{
			get { return ""; }
		}

		public DependencyObject NavigationContent
		{
			get { return null; }
		}
	}
}
