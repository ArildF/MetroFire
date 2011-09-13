using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public static class Extensions
	{
		public static void SubscribeThreadPool<T>(this IObservable<T> bus, Action<T> listener)
		{
			if (RxApp.DeferredScheduler == Scheduler.Immediate)
			{
				bus.ObserveOn(Scheduler.Immediate).Subscribe(listener);
			}
			else
			{
				bus.ObserveOn(Scheduler.ThreadPool).Subscribe(listener);
			}
		}
	}
}
