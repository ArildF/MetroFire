using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		public static bool In<T>(this T self, params T[] comparands)
		{
			return comparands.Contains(self);
		}

		public static bool IsRecoverable(this WebException exception)
		{
			return exception.Status.In(WebExceptionStatus.Timeout) ||
				(exception.Status.In(WebExceptionStatus.ProtocolError) &&
					((HttpWebResponse)exception.Response).StatusCode.In(
							HttpStatusCode.InternalServerError,
							HttpStatusCode.BadGateway));
		}
	}
}
