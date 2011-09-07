using Castle.Core;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient
{
	public class Campfire : ICampfire, IStartable
	{
		private readonly IMessageBus _bus;
		private readonly ICampfireApi _api;

		private Account _account;


		public Campfire(IMessageBus bus, ICampfireApi api)
		{
			_bus = bus;
			_api = api;

		}

		public IAccount Account
		{
			get { return _account; }
		}

		private void Subscribe()
		{
			_bus.Listen<RequestLoginMessage>().SubscribeThreadPool(StartLogin);
		}

		private void StartLogin(RequestLoginMessage requestLoginMessage)
		{
			_api.SetLoginInfo(requestLoginMessage.LoginInfo);
			_account = _api.GetAccountInfo();

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