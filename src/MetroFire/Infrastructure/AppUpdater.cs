﻿using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Reactive.Linq;
using Castle.Core;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;

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

			
			var updateCheckCompleted = Observable.FromEventPattern<CheckForUpdateCompletedEventArgs>(_deployment,
					"CheckForUpdateCompleted");

			_disposables.Add(
				_bus.RegisterMessageSource(
					updateCheckCompleted.Where(e => e.EventArgs.Error != null)
					.Select(e => new ExceptionMessage(e.EventArgs.Error))));

			var updateAvailable = updateCheckCompleted
				.Where(e => e.EventArgs.Error == null)
				.Where(e => e.EventArgs.UpdateAvailable)
				.Select(_ => new AppUpdateAvailableMessage()).Take(1);

			_disposables.Add(_bus.RegisterMessageSource(updateAvailable));


			var initialCheck = Observable.Timer(TimeSpan.FromSeconds(15));

			_disposables.Add(Observable.Interval(Properties.Settings.Default.UpdateCheckInterval)
				.Merge(initialCheck)
				.TakeUntil(updateAvailable)
				.Subscribe(_ => _deployment.CheckForUpdateAsync()));

			_disposables.Add(_bus.Listen<RequestAppUpdateMessage>()
				.SkipUntil(updateAvailable)
				.Take(1)
				.ObserveOn(RxApp.TaskpoolScheduler).
				Subscribe(_ => _deployment.UpdateAsync()));

			_disposables.Add(Observable.FromEventPattern<AsyncCompletedEventArgs>(_deployment, "UpdateCompleted")
				.ObserveOn(RxApp.TaskpoolScheduler).Subscribe(e =>
					{
						if (e.EventArgs.Error != null)
						{
							_bus.SendMessage(new ExceptionMessage(e.EventArgs.Error));
						}
						else
						{
							_bus.SendMessage(new RequestApplicationRestartMessage());
						}

					}));

			_disposables.Add(_bus.RegisterMessageSource(
				Observable.FromEventPattern<DeploymentProgressChangedEventArgs>(_deployment, "UpdateProgressChanged")
					.Select(e => new AppUpdateProgressMessage(e.EventArgs.ProgressPercentage))));
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
