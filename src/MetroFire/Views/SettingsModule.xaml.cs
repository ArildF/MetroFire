using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for SettingsModule.xaml
	/// </summary>
	public partial class SettingsModule : IMainModule
	{
		private readonly FrameworkElement _navigationContent;

		private SettingsModule()
		{
			InitializeComponent();
		}

		public SettingsModule(ISettingsViewModel vm) : this()
		{
			DataContext = vm;

			_navigationContent = (FrameworkElement)FindResource("BackButton");
			_navigationContent.DataContext = vm;
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
			get { return _navigationContent; }
		}
	}
}
