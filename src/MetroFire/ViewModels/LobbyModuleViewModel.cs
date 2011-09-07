﻿using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LobbyModuleViewModel : ReactiveObject, ILobbyModuleViewModel
	{
		private readonly IMessageBus _messageBus;
		private IAccount _account;

		public LobbyModuleViewModel(ICampfire campfire, IMessageBus messageBus)
		{
			_messageBus = messageBus;
			_account = campfire.Account;

			_messageBus.Listen<AccountUpdated>()
				.SubscribeUI(msg =>
				{
					_account = msg.Account;
					this.RaisePropertyChanged(vm => vm.AccountName);
					this.RaisePropertyChanged(vm => vm.AccountPlan);
					this.RaisePropertyChanged(vm => vm.AccountStorage);
					this.RaisePropertyChanged(vm => vm.AccountSubdomain);
				});
		}

		public string AccountName
		{
			get { return _account.Name; }
		}

		public string AccountPlan
		{
			get { return _account.Plan; }
		}
		public string AccountSubdomain
		{
			get { return _account.Subdomain; }
		}
		public string AccountStorage
		{
			get { return _account.Storage; }
		}
	}
}
