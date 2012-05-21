using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : IMainModule
	{
		private readonly INavigationContent _content;
		private LoginView()
		{
			InitializeComponent();
		}


		public LoginView(ILoginViewModel viewModel, INavigationContent content) : this()
		{
			_content = content;
			DataContext = viewModel;
		}

		public string Caption
		{
			get { return ""; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}

		public bool IsActive
		{
			get { return true; }
			set{}
		}

		public int Id
		{
			get { return -1; }
		}

		public string Notifications
		{
			get { return ""; }
		}

		public bool Closable
		{
			get { return false; }
		}

		public DependencyObject NavigationContent
		{
			get { return _content.Visual; }
		}
	}
}
