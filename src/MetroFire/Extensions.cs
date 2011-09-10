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

		public static void SubscribeOnceUI<T>(this IObservable<T> self, Action<T> onNext)
		{
			IDisposable disposable = null;
			disposable = self.ObserveOn(RxApp.DeferredScheduler).Subscribe(
				msg =>
					{
						onNext(msg);
						if (disposable != null) disposable.Dispose();
					}
				);
		}
	}
}
