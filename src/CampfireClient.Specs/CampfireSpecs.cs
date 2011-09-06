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

									_api.WhenToldTo(a => a.GetAccountInfo()).Return(new Account());

									_campfire = new Campfire(_bus, _api);

									_bus.Listen<LoginSuccessfulMessage>().Subscribe(msg => _loginSuccessfulMessageSent = true);
								};

		Because of = () => _bus.SendMessage(new RequestLoginMessage(_loginInfo));

		It should_set_login_info = () => _api.WasToldTo(a => a.SetLoginInfo(_loginInfo));
		It should_request_account_info = () => _api.WasToldTo(a => a.GetAccountInfo());

		It should_send_login_successful_message = () => _loginSuccessfulMessageSent.ShouldBeTrue();


		private static Campfire _campfire;
		private static MessageBus _bus;
		private static ICampfireApi _api;
		private static readonly LoginInfo _loginInfo = new LoginInfo("", "");
		private static bool _loginSuccessfulMessageSent;
	}
}
