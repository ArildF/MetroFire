using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Reactive.Linq;
using System.Reflection;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class FakeApplicationDeployment : IApplicationDeployment
	{
		public bool IsNetworkDeployed
		{
			get { return true; }
		}

		public void CheckForUpdateAsync()
		{
			Observable.Timer(TimeSpan.FromSeconds(2)).ObserveOn(RxApp.TaskpoolScheduler)
				.Take(1)
				.Subscribe(_ =>
				{
					if (CheckForUpdateCompleted != null)
					{
						var args = CreateCheckForUpdateCompletedEventArgs(null);
						CheckForUpdateCompleted(this, args);

					}
				});
		}

		public void UpdateAsync()
		{
			int i = 1;
			Observable.Interval(TimeSpan.FromSeconds(0.1))
				.Take(100)
				.Subscribe(
					t => RaiseUpdateProgress(i++),
					RaiseUpdateCompleted);
		}

		private void RaiseUpdateCompleted()
		{
			var args = CreateUpdateCompletedEventArgs();
			if (UpdateCompleted != null)
			{
				UpdateCompleted(this, args);
			}
		}

		private AsyncCompletedEventArgs CreateUpdateCompletedEventArgs()
		{
			var args = new AsyncCompletedEventArgs(null, false, null);
			return args;
		}

		private void RaiseUpdateProgress(int percent)
		{
			var args = CreateDeploymentProgressChangedEventArgs(percent);
			if (UpdateProgressChanged != null)
			{
				UpdateProgressChanged(this, args);
			}
		}

		private DeploymentProgressChangedEventArgs CreateDeploymentProgressChangedEventArgs(int percent)
		{
			var ctor =typeof(DeploymentProgressChangedEventArgs).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, null, 
				new []{typeof(int), typeof(object), typeof(long), typeof(long), typeof(DeploymentProgressState), typeof(string)},
				null);

			var args = ctor.Invoke(new object[] {percent, null, 0L, 0L, DeploymentProgressState.DownloadingApplicationFiles, "Blah"});
			return (DeploymentProgressChangedEventArgs) args;
		}

		public event CheckForUpdateCompletedEventHandler CheckForUpdateCompleted;

		private static CheckForUpdateCompletedEventArgs CreateCheckForUpdateCompletedEventArgs(Exception error)
		{
			var ctor = typeof (CheckForUpdateCompletedEventArgs).GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance, null, 
				new[] { typeof (Exception), typeof (bool), typeof (object), typeof (bool), typeof (Version), typeof (bool), typeof (Version), typeof (long) }, 
				null);
			var args = ctor.Invoke(new object[] {error, false, null, true, new Version(2, 0), true, new Version(1, 0), 1234L});
			return (CheckForUpdateCompletedEventArgs) args;
		}

		public event AsyncCompletedEventHandler UpdateCompleted;
		public event DeploymentProgressChangedEventHandler UpdateProgressChanged;


		public void Update()
		{
			
		}
	}
}