using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public static class Extensions
	{
		public static IDisposable SubscribeThreadPool<T>(this IObservable<T> bus, Action<T> listener)
		{
			if (RxApp.DeferredScheduler == Scheduler.Immediate)
			{
				return bus.ObserveOn(Scheduler.Immediate).Subscribe(listener);
			}
			else
			{
				return bus.ObserveOn(Scheduler.ThreadPool).Subscribe(listener);
			}
		}
	}
}
