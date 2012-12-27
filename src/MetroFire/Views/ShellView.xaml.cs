using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using ReactiveUI;
using Rogue.MetroFire.UI.Behaviors;

namespace Rogue.MetroFire.UI.Views
{
	public partial class ShellView : IShellWindow
	{
		private readonly IMessageBus _bus;
		private readonly IModuleResolver _resolver;
		private readonly ISettings _settings;
		private readonly Stack<IMainModule> _navigationStack = new Stack<IMainModule>();

		protected ShellView()
		{
			InitializeComponent();

			Loaded += OnLoaded;
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

		public ShellView(IShellViewModel vm, IMessageBus bus, IModuleResolver resolver, ISettings settings) : this()
		{
			_bus = bus;
			_resolver = resolver;
			_settings = settings;
			DataContext = vm;

			_bus.Listen<ActivateMainModuleMessage>().Subscribe(msg => ActivateMainModule(msg.ModuleName));
			_bus.Listen<NavigateMainModuleMessage>().Subscribe(msg => NavigateMainModule(msg.ModuleName));
			_bus.Listen<NavigateBackMainModuleMessage>().Where(_ => _navigationStack.Count > 0).Subscribe(
				_ => NavigateBack());

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

		private void NavigateBack()
		{
			ReleaseCurrentMainModule();
			var previousModule = _navigationStack.Pop();
			ActivateNewModule(previousModule);
		}

		private void NavigateMainModule(string moduleName)
		{
			var currentModule = _mainContent.Content as IMainModule;
			if (currentModule != null)
			{
				_navigationStack.Push(currentModule);
			}

			var newModule = (IMainModule)_resolver.ResolveModule(moduleName);
			ActivateNewModule(newModule);
		}

		private void ActivateMainModule(string moduleName)
		{
			ReleaseCurrentMainModule();
			var newModule = (IMainModule)_resolver.ResolveModule(moduleName);
			ActivateNewModule(newModule);
		}

		private void ReleaseCurrentMainModule()
		{
			var currentModule = _mainContent.Content as IMainModule;
			if (currentModule != null)
			{
				_resolver.ReleaseModule(currentModule);
			}
		}

		private void ActivateNewModule(IMainModule newModule)
		{
			_mainContent.Content = newModule.Visual;
			_navigationContent.Content = newModule.NavigationContent;
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
	}
}
