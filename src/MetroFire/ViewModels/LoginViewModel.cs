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

		public LoginViewModel(IMessageBus bus)
		{
			_bus = bus;

			Func<IObservedChange<LoginViewModel, string>, bool> isValid = c => !String.IsNullOrEmpty(c.Value);
			LoginCommand = new ReactiveCommand(
				Observable.Merge(
					this.ObservableForProperty(vm => vm.Account).Select(isValid),
					this.ObservableForProperty(vm => vm.Token).Select(isValid)
					));

			_bus.RegisterMessageSource(
				LoginCommand.Select(_ => new RequestLoginMessage(new LoginInfo(Account, Token))));

			_bus.RegisterMessageSource(
				_bus.Listen<LoginSuccessfulMessage>().Select(msg => new ActivateMainModuleMessage(ModuleNames.MainCampfireView)));
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

	}
}
