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

			var requestUploadMessage = new RequestUploadMessage(message.RoomId, message.Id);
			_bus.Listen<UploadReceivedMessage>().Where(msg => msg.Correlation == requestUploadMessage.Correlation)
				.SubscribeUI(UploadReceived);
			_bus.SendMessage(requestUploadMessage);
		}

		public object Data
		{
			get { return _data; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Data, ref _data, value); }
		}

		private void UploadReceived(UploadReceivedMessage uploadReceivedMessage)
		{
			var upload = uploadReceivedMessage.Upload;
			if (upload.ContentType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
			{
				Data = new InlineImageViewModel(uploadReceivedMessage.Upload, _bus);
			}
			else
			{
				Data = new FileViewModel(upload);
			}
		}
	}

	public class FileViewModel : ReactiveObject
	{
		private readonly Upload _upload;

		public FileViewModel(Upload upload)
		{
			_upload = upload;
		}

		protected Upload Upload
		{
			get { return _upload; }
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

	public class InlineImageViewModel : FileViewModel
	{
		private string _file;

		public InlineImageViewModel(Upload upload, IMessageBus bus) : base(upload)
		{
			bus.Listen<FileDownloadedMessage>().Where(msg => msg.Url == Upload.FullUrl)
				.SubscribeUI(msg => File = msg.File);
			bus.SendMessage(new RequestDownloadFileMessage(Upload.FullUrl));
		}

		public string File
		{
			get { return _file; }
			private set
			{
				this.RaiseAndSetIfChanged(vm => vm.File, ref _file, value);
			}
		}
	}
}
