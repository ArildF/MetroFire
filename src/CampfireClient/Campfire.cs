using Castle.Core;
using ReactiveUI;
using System;

namespace Rogue.MetroFire.CampfireClient
{
	public class Campfire : ICampfire, IStartable
	{
		private readonly IMessageBus _bus;
		private readonly ICampfireApi _api;

		public Campfire(IMessageBus bus, ICampfireApi api)
		{
			_bus = bus;
			_api = api;

		}

		private void Subscribe()
		{
			_bus.Listen<RequestLoginMessage>().Subscribe(StartLogin);
		}

		private void StartLogin(RequestLoginMessage requestLoginMessage)
		{
			_api.SetLoginInfo(requestLoginMessage.LoginInfo);
			var account = _api.GetAccountInfo();

			_bus.SendMessage<LoginSuccessfulMessage>(null);
		}

		public void Start()
		{
			Subscribe();
		}

		public void Stop()
		{
			
		}
	}
}