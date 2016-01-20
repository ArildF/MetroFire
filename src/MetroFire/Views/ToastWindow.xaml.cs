using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for ToastWindow.xaml
	/// </summary>
	public partial class ToastWindow : IToastWindow
	{
		private IWebBrowser _browser;

		private ToastWindow()
		{
			InitializeComponent();

			SizeChanged += OnSizeChanged;

			AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(RequestNavigateToLink));
		}

		private void RequestNavigateToLink(object sender, RequestNavigateEventArgs requestNavigateEventArgs)
		{
			_browser.NavigateTo(requestNavigateEventArgs.Uri);
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			Top = 0;
			Height = SystemParameters.MaximizedPrimaryScreenHeight - SystemParameters.CaptionHeight;
			Left = SystemParameters.PrimaryScreenWidth - ActualWidth;
		}

		public ToastWindow(IToastWindowViewModel vm, IWebBrowser browser) : this()
		{
			DataContext = vm;
			_browser = browser;
		}

		public Window Window
		{
			get { return this; }
		}
	}

}
