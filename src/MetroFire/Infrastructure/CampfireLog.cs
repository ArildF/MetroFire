using System;
using System.Reactive;
using System.Reactive.Subjects;
using Castle.Core;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class CampfireLog : ICampfireLog, IStartable
	{
		private readonly Subject<Unit> _updated = new Subject<Unit>();

		private readonly IMessageBus _bus;
		private IDisposable _disposable;

		public CampfireLog(IMessageBus bus)
		{
			_bus = bus;
		}

		public string Text { get; private set; }

		public IObservable<Unit> Updated
		{
			get { return _updated; }
		}

		public void Start()
		{
			_disposable = _bus.Listen<ExceptionMessage>().Subscribe(HandleExceptionMessage);
			_bus.Listen<LogMessage>().Subscribe(HandleLogMessage);
		}

		public void Stop()
		{
			_disposable.Dispose();
		}

		private void HandleLogMessage(LogMessage msg)
		{
			Text += string.Format("{0} [{1}]:{3} {2}{3}", DateTime.Now.ToShortTimeString(), msg.LogMessageType.ToString().ToUpper(),
				msg.Text, Environment.NewLine);

			_updated.OnNext(Unit.Default);
		}

		private void HandleExceptionMessage(ExceptionMessage msg)
		{
			Text += string.Format("{0} [{1}]:{3} {2}{3}", DateTime.Now.ToShortTimeString(), "EXCEPTION", 
				msg.Exception, Environment.NewLine);

			_updated.OnNext(Unit.Default);
		}
	}
}
