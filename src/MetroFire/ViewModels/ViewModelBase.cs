using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class ViewModelBase : ReactiveObject, IDisposable
	{
		private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

		protected void Subscribe(Func<IDisposable> disposable)
		{
			_subscriptions.Add(disposable());
		}

		protected void Subscribe(IDisposable disposable)
		{
			_subscriptions.Add(disposable);
		}

		public void Dispose()
		{
			foreach (var subscription in _subscriptions)
			{
				subscription.Dispose();
			}
		}
	}
}
