using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.CampfireClient
{
	public static class Extensions
	{
		public static void ListenThreadPool<T>(this IObservable<T> bus, Action<T> listener)
		{
			bus.ObserveOn(Scheduler.Immediate).Subscribe(listener);
		}
	}
}
