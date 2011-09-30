using System;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class InlineUploadViewModel : ReactiveObject, IInlineUploadViewModel
	{
		private readonly IMessageBus _bus;
		private object _data;

		public InlineUploadViewModel(IMessageBus bus, Message message)
		{
			_bus = bus;

			_bus.Listen<UploadReceivedMessage>().Where(msg => msg.Upload.RoomId == message.RoomId)
				.SubscribeUI(UploadReceived);
			_bus.SendMessage(new RequestUploadMessage(message.RoomId, message.Id));
		}

		public object Data
		{
			get { return _data; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Data, ref _data, value); }
		}

		private void UploadReceived(UploadReceivedMessage uploadReceivedMessage)
		{
			Data = new InlineImageViewModel(uploadReceivedMessage.Upload, _bus);
		}
	}

	public class InlineImageViewModel : ReactiveObject
	{
		private readonly Upload _upload;
		private string _file;
		private bool _isEnabled;

		public InlineImageViewModel(Upload upload, IMessageBus bus)
		{
			_upload = upload;

			bus.Listen<FileDownloadedMessage>().Where(msg => msg.Url == _upload.FullUrl)
				.SubscribeUI(msg => File = msg.File);
			bus.SendMessage(new RequestDownloadFileMessage(_upload.FullUrl));
		}

		public string File
		{
			get { return _file; }
			private set
			{
				this.RaiseAndSetIfChanged(vm => vm.File, ref _file, value);
			}
		}

		public string FullUrl
		{
			get { return _upload.FullUrl; }
		}

		public string Name
		{
			get { return _upload.Name; }
		}

		

	}
}
