using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml
	/// </summary>
	public partial class ShellView : IShellWindow
	{
		private readonly IMessageBus _bus;
		private readonly IModuleResolver _resolver;
		private readonly Stack<IMainModule> _navigationStack = new Stack<IMainModule>();

		protected ShellView()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_bus.SendMessage<ApplicationLoadedMessage>(null);
		}

		public ShellView(IShellViewModel vm, IMessageBus bus, IModuleResolver resolver) : this()
		{
			_bus = bus;
			_resolver = resolver;
			DataContext = vm;

			_bus.Listen<ActivateMainModuleMessage>().Subscribe(msg => ActivateMainModule(msg.ModuleName));
			_bus.Listen<NavigateMainModuleMessage>().Subscribe(msg => NavigateMainModule(msg.ModuleName));
			_bus.Listen<NavigateBackMainModuleMessage>().Where(_ => _navigationStack.Count > 0).Subscribe(
				_ => NavigateBack());



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
			DragMove();
		}


		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
		}

	}
}
