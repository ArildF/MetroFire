using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace Rogue.MetroFire.UI
{
	public static class Extensions
	{
		public static void SubscribeUI<T>(this IObservable<T> self, Action<T> onNext)
		{
			self.ObserveOn(RxApp.DeferredScheduler).Subscribe(onNext);
		}
	}
}
