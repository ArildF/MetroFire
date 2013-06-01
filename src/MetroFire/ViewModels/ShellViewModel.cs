using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;
using System;
using ReactiveUI.Xaml;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IModuleResolver _resolver;
		private int _unreadCount;
		private const string DefaultTitle = "metro fire";
		private string _title;
		private object _currentModule;

		private readonly Stack<IMainModule> _navigationStack = new Stack<IMainModule>();
		private object _navigationContent;

		public ShellViewModel(IMessageBus bus, IModuleResolver resolver)
		{
			_resolver = resolver;
			Title = DefaultTitle;

			var activated = bus.Listen<ApplicationActivatedMessage>();
			var deactivated = bus.Listen<ApplicationDeactivatedMessage>();

			bus.Listen<RoomActivityMessage>()
				.SkipUntil(deactivated)
				.TakeUntil(activated.Do(_ => UnreadCount = 0))
				.Repeat() 
				.Subscribe(_ => UnreadCount++);

			bus.Listen<RequestApplicationRestartMessage>().ObserveOnDispatcher().Subscribe(_ =>
				{
					System.Windows.Forms.Application.Restart();
					Environment.Exit(1);
				});

			bus.Listen<ActivateMainModuleMessage>().Subscribe(msg => ActivateMainModule(msg.ModuleName));
			bus.Listen<NavigateMainModuleMessage>().Subscribe(msg => NavigateMainModule(msg.ModuleName));
			bus.Listen<NavigateBackMainModuleMessage>().Where(_ => _navigationStack.Count > 0).Subscribe(
				_ => NavigateBack());

		}

		public ReactiveCommand NextModuleCommand { get; set; }


		public object CurrentModule
		{
			get { return _currentModule; }
			set { this.RaiseAndSetIfChanged(vm => vm.CurrentModule, ref _currentModule, value); }
		}

		public object NavigationContent
		{
			get { return _navigationContent; }
			set { this.RaiseAndSetIfChanged(vm => vm.NavigationContent, ref _navigationContent, value); }
		}

		public int UnreadCount
		{
			get {
				return _unreadCount;
			}
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.UnreadCount, ref _unreadCount, value);

				Title = _unreadCount > 0 ? string.Format("({0}) {1}", _unreadCount, DefaultTitle) : DefaultTitle;
			}
		}

		public string Title
		{
			get { return _title; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Title, ref _title, value); }
		}

		private void NavigateBack()
		{
			ReleaseCurrentMainModule();
			var previousModule = _navigationStack.Pop();
			ActivateNewModule(previousModule);
		}

		private void NavigateMainModule(string moduleName)
		{
			var currentModule = CurrentModule as IMainModule;
			if (currentModule != null)
			{
				_navigationStack.Push(currentModule);
			}

			var newModule = _resolver.ResolveMainModule(moduleName);
			ActivateNewModule(newModule);
		}

		private void ActivateMainModule(string moduleName)
		{
			ReleaseCurrentMainModule();
			var newModule = _resolver.ResolveMainModule(moduleName);
			ActivateNewModule(newModule);
		}

		private void ReleaseCurrentMainModule()
		{
			var currentModule = CurrentModule as IMainModule;
			if (currentModule != null)
			{
				_resolver.ReleaseModule(currentModule);
			}
		}

		private void ActivateNewModule(IMainModule newModule)
		{
			CurrentModule = newModule;
			NavigationContent = newModule.NavigationContent;
		}

	}
}
