using System.Windows.Media.Imaging;
using Castle.Core;
using ReactiveUI;
using System;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class PasteHandler : IStartable
	{
		private readonly IMessageBus _bus;
		private readonly IPasteViewFactory _pasteViewFactory;

		public PasteHandler(IMessageBus bus, IPasteViewFactory pasteViewFactory)
		{
			_bus = bus;
			_pasteViewFactory = pasteViewFactory;
		}

		public void Start()
		{
			_bus.Listen<PasteImageMessage>().Subscribe(OnPasteImage);
		}

		private void OnPasteImage(PasteImageMessage pasteImageMessage)
		{
			var view = _pasteViewFactory.Create(pasteImageMessage.Room, pasteImageMessage.Image);
			view.ShowDialog();

			_pasteViewFactory.Release(view);
		}

		public void Stop()
		{
		}
	}
}
