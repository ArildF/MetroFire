using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LoginViewModel : ReactiveObject, ILoginViewModel
	{
		private readonly IMessageBus _bus;
		private string _account;
		private string _token;
		private bool _isLoggingIn;

		public LoginViewModel(IMessageBus bus, ILoginInfoStorage storage)
		{
			_bus = bus;

			Func<IObservedChange<LoginViewModel, string>, bool> isValid = c => !String.IsNullOrEmpty(c.Value);
			LoginCommand = new ReactiveCommand(
				Observable.CombineLatest(
					this.ObservableForProperty(vm => vm.Account).Select(isValid),
					this.ObservableForProperty(vm => vm.Token).Select(isValid),
					(b1, b2) => b1 && b2
					));

			_bus.RegisterMessageSource(
				LoginCommand.Select(_ => new RequestLoginMessage(new LoginInfo(Account, Token))).Do(_ => IsLoggingIn = true));

			_bus.RegisterMessageSource(
				_bus.Listen<LoginSuccessfulMessage>()
				.Do(_ => storage.PersistLoginInfo(new LoginInfo(Account, Token)))
				.Do(_ => IsLoggingIn = false)
				.Select(msg => new ActivateMainModuleMessage(ModuleNames.MainCampfireView)));

			LoginInfo info = storage.GetStoredLoginInfo();
			if (info != null)
			{
				Token = info.Token;
				Account = info.Account;
			}
		}


		public ReactiveCommand LoginCommand { get; private set; }

		public string Account
		{
			get { return _account; }
			set { this.RaiseAndSetIfChanged(vm => vm.Account, ref _account, value); }
		}

		public string Token
		{
			get { return _token; }
			set { this.RaiseAndSetIfChanged(vm => vm.Token, ref _token, value); }
		}

		public bool IsLoggingIn
		{
			get { return _isLoggingIn; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsLoggingIn, ref _isLoggingIn, value); }
		}

	}
}
