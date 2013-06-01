using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LoginViewModel : ReactiveObject, IMainModule
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
		private bool _isTokenInError;

		public LoginViewModel(IMessageBus bus, ILoginInfoStorage storage, INavigationContentViewModel navigationContent)
		{
			_bus = bus;

			NavigationContent = navigationContent;

			_isTokenValid = this.ObservableToProperty(
				this.ObservableForProperty(vm => vm.Token).Select(t => !String.IsNullOrEmpty(t.Value)), 
				vm => vm.IsTokenValid);
			LoginCommand = new ReactiveCommand(
				this.WhenAny(
					vm => vm.IsAccountNameVerified,
					vm => vm.IsTokenValid,
					(verified, token) => verified.Value && token.Value
					));
			_bus.RegisterSourceAndHandleReply<RequestLoginMessage, RequestLoginResponse>(
				LoginCommand.Select(_ => new RequestLoginMessage(new LoginInfo(Account, Token)))
					.Do(_ => IsLoggingIn = true)
					.Do(_ => IsTokenInError = false),

				res =>
					{
						IsLoggingIn = false;
						if (res.SuccessFul)
						{
							storage.PersistLoginInfo(new LoginInfo(Account, Token));
							IsTokenInError = false;
							_bus.SendMessage(new ActivateMainModuleMessage(ModuleNames.MainCampfireView));
						}
						else
						{
							IsTokenInError = true;
						}
					});


			RetryCommand = new ReactiveCommand();
			
			_bus.RegisterSourceAndHandleReply<RequestCheckAccountName, RequestCheckAccountNameReply>(
				this.ObservableForProperty(vm => vm.Account)
				.DistinctUntilChanged()
				.Select(_ => Unit.Default)
				.Throttle(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
				.Merge(RetryCommand.Select(_ => Unit.Default))
				.Do(_ =>
				{
					IsAccountNameVerified = false;
					IsAccountNameInError = String.IsNullOrEmpty(Account);
					ShowConnectionError = false;
				})
				.Where(_ => !String.IsNullOrEmpty(Account))
				.Select(_ => new RequestCheckAccountName(Account))
				.Do(_ => IsVerifyingAccountInProgress = true)
				.Do(msg => _accountNameCorrelationId = msg.CorrelationId),

				rep =>
				{
					IsVerifyingAccountInProgress = false;
					IsAccountNameVerified = rep.Result;
					IsAccountNameInError = !rep.Result;
				},

				ex =>
				{
					IsVerifyingAccountInProgress = false;
					IsAccountNameVerified = false;
					IsAccountNameInError = true;
					ShowConnectionError = true;
					ConnectionErrorMessage = ex.Message;
				});

			ProxySettingsCommand = new ReactiveCommand();
			_bus.RegisterMessageSource(ProxySettingsCommand.Select(_ => new NavigateSettingsPageMessage(SettingsPageNames.Network)));

			LoginInfo info = storage.GetStoredLoginInfo();
			if (info != null)
			{
				Token = info.Token;
				Account = info.Account;
			}
		}


		public ReactiveCommand RetryCommand { get; private set; }
		public ReactiveCommand ProxySettingsCommand { get; private set; }

		public ReactiveCommand LoginCommand { get; private set; }

		private readonly ObservableAsPropertyHelper<bool> _isTokenValid;


		public bool IsTokenInError
		{
			get { return _isTokenInError; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsTokenInError, ref _isTokenInError, value); }
		}

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


		public string Caption { get { return ""; } }
		public bool IsActive { get; set; }
		public int Id { get { return -1; } }
		public string Notifications { get { return ""; } }
		public bool Closable { get { return false; } }
		public object NavigationContent { get; private set; }
	}
}
