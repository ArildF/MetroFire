using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class ToastViewModel : ViewModelBase
	{
		private readonly IChatDocument _document;
		private bool _isClosing;
		private bool _isVisible;



		public ToastViewModel(ShowToastMessage showToastMessage, IChatDocument document, IMessageBus bus, IApplicationActivator activator)
		{
			_document = document;
			_document.FontSize = 14;
			IsVisible = true;
			RoomName = showToastMessage.Message.Room.Name;
			_document.AddMessage(showToastMessage.Message.Message, showToastMessage.Message.User, null);

			CloseCommand = new ReactiveCommand();
			ActivateCommand = new ReactiveCommand();

			Closed = Observable.Timer(TimeSpan.FromSeconds(showToastMessage.Action.SecondsVisible), RxApp.TaskpoolScheduler)
				 .Do(_ => IsClosing = true).Delay(TimeSpan.FromSeconds(2), RxApp.TaskpoolScheduler)
				 .Select(_ => Unit.Default)
				 .Merge(CloseCommand.Select(_ => Unit.Default))
				 .Merge(ActivateCommand.Select(_ => Unit.Default));

			Subscribe(() =>bus.RegisterMessageSource(
				ActivateCommand.Select(
					_ => new ActivateModuleByIdMessage(ModuleNames.MainCampfireView, showToastMessage.Message.Room.Id))
					.Do(_ => activator.Activate())));
		}

		public ReactiveCommand CloseCommand { get; private set; }

		public IChatDocument Document
		{
			get { return _document; }
		}

		public IObservable<Unit> Closed { get; private set; }

		public string RoomName
		{
			get;
			private set;
		}
		

		public bool IsClosing
		{
			get { return _isClosing; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsClosing, ref _isClosing, value); }
		}

		public bool IsVisible
		{
			get { return _isVisible; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsVisible, ref _isVisible, value); }
		}

		public ReactiveCommand ActivateCommand { get; private set; }

	}
}