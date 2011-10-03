using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
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

		public static IObservable<Unit> GetDeactivated(this Application application)
		{
			return Observable.FromEventPattern((EventHandler<EventArgs> ev) => new EventHandler(ev), 
				ev => Application.Current.Deactivated += ev, 
				ev => Application.Current.Deactivated -= ev)
				.Select(_ => Unit.Default);
		}

		public static IObservable<Unit> GetActivated(this Application application)
		{
			return Observable.FromEventPattern((EventHandler<EventArgs> ev) => new EventHandler(ev),
				ev => Application.Current.Activated += ev,
				ev => Application.Current.Activated -= ev)
				.Select(_ => Unit.Default);
		}
	}
}
