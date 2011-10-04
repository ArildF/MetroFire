using System.Diagnostics;
using Castle.Core;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using System;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class LogMessageDebugListener : IStartable
	{
		private IMessageBus _bus;
		private IDisposable _subscription;

		public LogMessageDebugListener(IMessageBus bus)
		{
			_bus = bus;
		}

		public void Start()
		{
			_subscription = _bus.Listen<LogMessage>().Subscribe(OnNext);

		}

		private void OnNext(LogMessage logMessage)
		{
			Debug.WriteLine(String.Format("{0}: {1}", logMessage.LogMessageType, logMessage.Text));
		}

		public void Stop()
		{
			_subscription.Dispose();
			
		}
	}
}
