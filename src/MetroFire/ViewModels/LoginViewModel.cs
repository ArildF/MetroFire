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
		private bool _isAccountNameInError;
		private bool _isAccountNameVerified;
		private bool _isVerifyingAccountInProgress;

		public LoginViewModel(IMessageBus bus, ILoginInfoStorage storage)
		{
			_bus = bus;

			Func<IObservedChange<LoginViewModel, string>, bool> isValid = c => !String.IsNullOrEmpty(c.Value);
			LoginCommand = new ReactiveCommand(
				Observable.CombineLatest(
					this.ObservableForProperty(vm => vm.IsAccountNameVerified).Select(c => c.Value),
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

			_bus.RegisterMessageSource(this.ObservableForProperty(vm => vm.Account)
				.DistinctUntilChanged()
				.Do(_ => 
				{
					IsAccountNameVerified = false;
					IsAccountNameInError = false;
					IsVerifyingAccountInProgress = true;
				})
				.Throttle(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
				.Select(_ => new RequestCheckAccountName(Account)));

			_bus.Listen<RequestCheckAccountNameReply>().Subscribe(rep =>
				{
					IsVerifyingAccountInProgress = false;
					IsAccountNameVerified = rep.Result;
					IsAccountNameInError = !rep.Result;
				});

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

		public bool IsAccountNameInError
		{
			get { return _isAccountNameInError; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsAccountNameInError, ref _isAccountNameInError, value); }
		}

		public bool IsAccountNameVerified
		{
			get { return _isAccountNameVerified; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsAccountNameVerified, ref _isAccountNameVerified, value); }
		}

		public bool IsVerifyingAccountInProgress
		{
			get { return _isVerifyingAccountInProgress; }
			private set{ this.RaiseAndSetIfChanged(vm => vm.IsVerifyingAccountInProgress, ref _isVerifyingAccountInProgress, value);}
		}
	}
}
