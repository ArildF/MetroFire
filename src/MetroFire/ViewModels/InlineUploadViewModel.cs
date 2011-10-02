using System;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;
using Rogue.MetroFire.UI.Views;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class InlineUploadViewModel : ReactiveObject, IInlineUploadViewModel
	{
		private readonly IMessageBus _bus;
		private readonly Func<string, IImageView> _imageViewCreator;
		private object _data;

		public InlineUploadViewModel(IMessageBus bus, Message message, Func<string, IImageView> imageViewCreator)
		{
			_bus = bus;
			_imageViewCreator = imageViewCreator;

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
				Data = new InlineImageViewModel(uploadReceivedMessage.Upload, _bus, _imageViewCreator);
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
		private readonly Func<string, IImageView> _imageViewCreator;
		private string _file;

		public InlineImageViewModel(Upload upload, IMessageBus bus, Func<string, IImageView> imageViewCreator) : base(upload)
		{
			_imageViewCreator = imageViewCreator;
			ShowFullSizeImageCommand = new ReactiveCommand(
				this.ObservableForProperty(vm => vm.File).Select(c => c.Value != null));
			ShowFullSizeImageCommand.Subscribe(ViewImage);
			bus.Listen<FileDownloadedMessage>().Where(msg => msg.Url == Upload.FullUrl)
				.SubscribeUI(msg => File = msg.File);
			bus.SendMessage(new RequestDownloadFileMessage(Upload.FullUrl));


		}

		private void ViewImage(object o)
		{
			var imageView = _imageViewCreator(File);
			imageView.ShowDialog();
		}

		public ReactiveCommand ShowFullSizeImageCommand { get; private set; }

		public string File
		{
			get { return _file; }
			private set
			{
				this.RaiseAndSetIfChanged(vm => vm.File, ref _file, value);
			}
		}
	}

	public class ShowFullSizeImageMessage
	{
		public string File { get; private set; }

		public ShowFullSizeImageMessage(string file)
		{
			File = file;
		}
	}
}
