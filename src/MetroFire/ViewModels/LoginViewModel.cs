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
		private bool _showConnectionError;
		private Guid _accountNameCorrelationId;
		private string _connectionErrorMessage;

		public LoginViewModel(IMessageBus bus, ILoginInfoStorage storage)
		{
			_bus = bus;

			_isTokenValid = this.ObservableToProperty(
				this.ObservableForProperty(vm => vm.Token).Select(t => !String.IsNullOrEmpty(t.Value)), 
				vm => vm.IsTokenValid);
			LoginCommand = new ReactiveCommand(
				this.WhenAny(
					vm => vm.IsAccountNameVerified,
					vm => vm.IsTokenValid,
					(verified, token) => verified.Value && token.Value
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
					IsAccountNameInError = String.IsNullOrEmpty(Account);
					ShowConnectionError = false;
				})
				.Throttle(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
				.Where(_ => !String.IsNullOrEmpty(Account))
				.Select(_ => new RequestCheckAccountName(Account))
				.Do(_ => IsVerifyingAccountInProgress = true)
				.Do(msg => _accountNameCorrelationId = msg.CorrelationId)
				.Select(msg => msg));

			_bus.Listen<RequestCheckAccountNameReply>()
				.Where(msg => msg.CorrelationId == _accountNameCorrelationId)
				.SubscribeUI(rep =>
				{
					IsVerifyingAccountInProgress = false;
					IsAccountNameVerified = rep.Result;
					IsAccountNameInError = !rep.Result;
				});

			_bus.Listen<CorrelatedExceptionMessage>()
				.Where(msg => msg.CorrelationId == _accountNameCorrelationId)
				.SubscribeUI(msg =>
					{
						IsVerifyingAccountInProgress = false;
						IsAccountNameVerified = false;
						IsAccountNameInError = true;
						ShowConnectionError = true;
						ConnectionErrorMessage = msg.Exception.Message;
					});

			LoginInfo info = storage.GetStoredLoginInfo();
			if (info != null)
			{
				Token = info.Token;
				Account = info.Account;
			}
		}



		public ReactiveCommand LoginCommand { get; private set; }

		private ObservableAsPropertyHelper<bool> _isTokenValid;
		public bool IsTokenValid
		{
			get { return _isTokenValid.Value; }
		}
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

		public bool ShowConnectionError
		{
			get { return _showConnectionError; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ShowConnectionError, ref _showConnectionError, value); }
		}

		public string ConnectionErrorMessage
		{
			get { return _connectionErrorMessage; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ConnectionErrorMessage, ref _connectionErrorMessage, value); }
		}


	}
}
