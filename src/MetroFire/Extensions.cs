﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Castle.Core.Internal;

namespace Rogue.MetroFire.UI
{
	public static class Extensions
	{
		public static IDisposable SubscribeUI<T>(this IObservable<T> self, Action<T> onNext)
		{
			return self.ObserveOn(RxApp.DeferredScheduler).Subscribe(onNext);
		}

		public static IDisposable SubscribeUI<T>(this IObservable<T> self, Action<T> onNext, Action<Exception> onError)
		{
			return self.ObserveOn(RxApp.DeferredScheduler).Subscribe(onNext, onError);
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

		public static IDisposable RegisterSourceAndHandleReply<TRequest, TResponse>(
			this IMessageBus self,
			IObservable<TRequest> req, 
			Action<TResponse> responseHandler, Action<Exception> errorHandler = null)
			where TRequest : CorrelatedMessage where TResponse : CorrelatedReply
		{
			var coll = new DisposableCollection();
			Guid currentCorrelation = Guid.Empty;
			if (errorHandler != null)
			{
				coll.Add(self.Listen<CorrelatedExceptionMessage>().Where(msg => msg.CorrelationId == currentCorrelation)
					.SubscribeUI(msg => errorHandler(msg.Exception)));
			}
			coll.Add(self.Listen<TResponse>().Where(msg => msg.CorrelationId == currentCorrelation).SubscribeUI(responseHandler));

			coll.Add(self.RegisterMessageSource(req.Do(msg => currentCorrelation = msg.CorrelationId)));

			return coll;
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

		public static string AsKiloBytes(this long d)
		{
			return String.Format("{0:0.0}", d / 1024);
		}

		public static string AsMegaBytes(this long d)
		{
			return String.Format("{0:0.0}", d / 1024 / 1024);
		}

		public static long MegaBytes(this long megaBytes)
		{
			return megaBytes * 1024 * 1024;
		}

		public static long MegaBytes(this int megaBytes)
		{
			return ((long) megaBytes).MegaBytes();
		}

		public static long KiloBytes(this long kiloBytes)
		{
			return kiloBytes * 1024;
		}

		private class DisposableCollection : IDisposable
		{
			private readonly IList<IDisposable> _innerList = new List<IDisposable>();

			public void Add(IDisposable disposable)
			{
				_innerList.Add(disposable);
			}

			public void Dispose()
			{
				_innerList.ForEach(i => i.Dispose());
			}
		}

	}
}
