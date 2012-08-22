using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class FakeApplicationDeployment : IApplicationDeployment
	{
		private readonly Subject<AppUpdateProgressMessage> _progressSubject = 
			new Subject<AppUpdateProgressMessage>();

		public FakeApplicationDeployment()
		{
			UpdateAvailable = Observable.Timer(TimeSpan.FromSeconds(5), RxApp.TaskpoolScheduler).Select(_ => Unit.Default);
		}

		public bool IsNetworkDeployed
		{
			get { return true; }
		}

		public IObservable<Unit> UpdateAvailable { get; private set; }

		public IObservable<AppUpdateProgressMessage> UpdateProgress
		{
			get { return _progressSubject; }
		}

		public void Update()
		{
			var now = DateTimeOffset.Now;
			Observable.Generate(0, i => i < 100, i => i + 1, i => i, i => now + TimeSpan.FromSeconds(i * 0.1))
				.ObserveOn(Scheduler.ThreadPool).Select(i => new AppUpdateProgressMessage(i)).Subscribe(
					i => _progressSubject.OnNext(i), _ => Environment.Exit(0));
		}
	}
}