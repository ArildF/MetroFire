using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using ReactiveUI;
using Rogue.MetroFire.UI.Behaviors;

namespace Rogue.MetroFire.UI.Views
{
	public partial class ShellView : IShellWindow
	{
		private readonly IMessageBus _bus;
		private readonly ISettings _settings;
		private readonly IWebBrowser _browser;

		protected ShellView()
		{
			InitializeComponent();

			Loaded += OnLoaded;

			AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(RequestNavigateToLink));
		}

		private void RequestNavigateToLink(object sender, RequestNavigateEventArgs requestNavigateEventArgs)
		{
			_browser.NavigateTo(requestNavigateEventArgs.Uri);
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var behaviors = Interaction.GetBehaviors(this);
			if (!_settings.General.UseStandardWindowsChrome)
			{
				behaviors.Add(new BorderlessWindowBehavior());
			}

			_bus.SendMessage<ApplicationLoadedMessage>(null);
		}

		public ShellView(IShellViewModel vm, IMessageBus bus,  ISettings settings, IWebBrowser browser) : this()
		{
			_bus = bus;
			_settings = settings;
			_browser = browser;
			DataContext = vm;


			

			_bus.Listen<SettingsChangedMessage>().Subscribe(OnSettingsChanged);

		}

		private void OnSettingsChanged(SettingsChangedMessage settingsChangedMessage)
		{
			var behaviors = Interaction.GetBehaviors(this);
			if (_settings.General.UseStandardWindowsChrome && behaviors.OfType<BorderlessWindowBehavior>().Any())
			{
				behaviors.Remove(behaviors.OfType<BorderlessWindowBehavior>().First());
			}
			else if (!_settings.General.UseStandardWindowsChrome && !behaviors.OfType<BorderlessWindowBehavior>().Any())
			{
				behaviors.Add(new BorderlessWindowBehavior());
			}
		}

		
		public Window Window
		{
			get { return this; }
		}

		private void TopOnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}


		private void CloseOnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void MinimizeButtonOnClick(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}
	}
}
