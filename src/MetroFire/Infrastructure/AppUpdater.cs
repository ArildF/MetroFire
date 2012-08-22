using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using Castle.Core;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class AppUpdater : IStartable
	{
		private readonly DisposableCollection _disposables = new DisposableCollection();
		private readonly IApplicationDeployment _deployment;

		private readonly IMessageBus _bus;

		public AppUpdater(IMessageBus bus, IApplicationDeployment deployment)
		{
			_bus = bus;
			_deployment = deployment;
		}

		public void Start()
		{
			if (!_deployment.IsNetworkDeployed)
			{
				return;
			}


			_disposables.Add(_bus.RegisterMessageSource(
				_deployment.UpdateProgress));

			var updateAvailable = _deployment.UpdateAvailable.Select(_ => new AppUpdateAvailableMessage());
			var d = _bus.RegisterMessageSource(updateAvailable);
			_disposables.Add(d);

			_bus.Listen<RequestAppUpdateMessage>().ObserveOn(RxApp.TaskpoolScheduler).Subscribe(_ => _deployment.Update());
		}

		public void Stop()
		{
			if (!_deployment.IsNetworkDeployed)
			{
				return;
			}

			_disposables.Dispose();
		}
	}

}
