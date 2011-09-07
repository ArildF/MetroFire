using Castle.Core;
using Machine.Fakes;
using Machine.Specifications;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using System;

namespace Rogue.MetroFire.CampfireClient.Specs
{
	[Subject(typeof(Campfire))]
	public class When_sending_a_request_login_message_and_login_is_successful : WithFakes
	{
		Establish context = () =>
								{
									_api = An<ICampfireApi>();
									_bus = new MessageBus();

									_account = new Account();
									_api.WhenToldTo(a => a.GetAccountInfo()).Return(_account);

									_campfire = new Campfire(_bus, _api);
									((IStartable)_campfire).Start();

									_bus.Listen<LoginSuccessfulMessage>().Subscribe(msg => _loginSuccessfulMessageSent = true);
								};

		Because of = () => _bus.SendMessage(new RequestLoginMessage(_loginInfo));

		It should_set_login_info = () => _api.WasToldTo(a => a.SetLoginInfo(_loginInfo));
		It should_request_account_info = () => _api.WasToldTo(a => a.GetAccountInfo());

		It should_send_login_successful_message = () => _loginSuccessfulMessageSent.ShouldBeTrue();

		It should_expose_account = () => _campfire.Account.ShouldBeTheSameAs(_account);


		private static ICampfire _campfire;
		private static MessageBus _bus;
		private static ICampfireApi _api;
		private static readonly LoginInfo _loginInfo = new LoginInfo("", "");
		private static bool _loginSuccessfulMessageSent;
		private static Account _account;
	}
}
