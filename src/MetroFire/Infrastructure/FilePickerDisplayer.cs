using System;
using System.Windows.Forms;
using Castle.Core;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class FilePickerDisplayer : IStartable
	{
		private readonly IMessageBus _bus;
		private readonly IMimeTypeResolver _resolver;

		public FilePickerDisplayer(IMessageBus bus, IMimeTypeResolver resolver)
		{
			_bus = bus;
			_resolver = resolver;
			bus.Listen<RequestUploadFilePickerMessage>().Subscribe(ShowFilePicker);
		}

		private void ShowFilePicker(RequestUploadFilePickerMessage msg)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var type = _resolver.GetMimeType(dialog.FileName);
				var item = new FileItem(dialog.FileName, type);

				_bus.SendMessage(new UploadFilePickedMessage(item, msg.CorrelationId));

			}
		}

		public void Start()
		{
			
		}

		public void Stop()
		{
			throw new System.NotImplementedException();
		}
	}
}
