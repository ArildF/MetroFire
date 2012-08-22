using System;
using System.Deployment.Application;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using ReactiveUI;
using System.Windows.Forms;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ClickOnceApplicationDeployment : IApplicationDeployment
	{
		private readonly ApplicationDeployment _deployment;
		private bool _deploymentInProgress;
		private readonly object _deployLock = new object();


		public ClickOnceApplicationDeployment(IMessageBus bus)
		{
			if (ApplicationDeployment.IsNetworkDeployed)
			{
				_deployment = ApplicationDeployment.CurrentDeployment;
				UpdateAvailable = Observable.Interval(Properties.Settings.Default.UpdateCheckInterval)
					.Select(_ => 
					{
						if (Monitor.TryEnter(_deployLock))
						{
							try
							{
								return !_deploymentInProgress && _deployment.CheckForUpdate();
							}
							finally
							{
								Monitor.Exit(_deployLock);
							}
						}
						return false;
					})
					.Where(available => available)
					.Select(_ => Unit.Default);


				UpdateProgress =
					Observable.FromEventPattern<DeploymentProgressChangedEventArgs>(_deployment, "UpdateProgressChanged")
					.Select(e => e.EventArgs).Select(e => new AppUpdateProgressMessage(e.ProgressPercentage));

			}
		}
		public bool IsNetworkDeployed
		{
			get { return ApplicationDeployment.IsNetworkDeployed; }
		}

		public IObservable<Unit> UpdateAvailable { get; private set; }

		public IObservable<AppUpdateProgressMessage> UpdateProgress { get; private set; }
		public void Update()
		{
			lock (_deployLock)
			{
				_deploymentInProgress = true;
				var mre = new ManualResetEvent(false);
				Exception error = null;
				_deployment.UpdateCompleted += (sender, args) =>
					{
						error = args.Error;
						mre.Set();
					};
				_deployment.UpdateAsync();
				mre.WaitOne();

				if (error == null)
				{
					Application.Restart();
					Environment.Exit(0);
				}
			}
		}
	}
}
