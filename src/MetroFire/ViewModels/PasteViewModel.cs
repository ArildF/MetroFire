using System;
using System.Windows.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive.Linq;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class PasteViewModel : ReactiveObject, IPasteViewModel
	{
		private readonly BitmapSource _bitmapSource;
		private bool _isUploading;
		private string _uploadingPath;
		private bool _isFinished;

		public PasteViewModel(BitmapSource bitmapSource, IRoom room, IMessageBus bus, IImageEncoder encoder)
		{
			_bitmapSource = bitmapSource;

			PasteCommand = new ReactiveCommand();

			bus.Listen<FileUploadedMessage>().Where(msg => msg.Path == _uploadingPath)
				.Subscribe(_ => IsFinished = true);


			bus.RegisterMessageSource(
				PasteCommand.Do(_ => IsUploading = true)
				.Select(_ => encoder.EncodeToTempPng(bitmapSource))
				.Do(path => _uploadingPath = path)
				.Select(path => new RequestUploadFileMessage(room.Id, path, "image/png")));
		}

		public ReactiveCommand PasteCommand { get; private set; }

		public BitmapSource BitmapSource
		{
			get { return _bitmapSource; }
		}

		public bool IsUploading
		{
			get { return _isUploading; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsUploading, ref _isUploading, value); }
		}

		public bool IsFinished
		{
			get { return _isFinished; }
			private set { this.RaiseAndSetIfChanged(vm => vm.IsFinished, ref _isFinished, value); }
		}

	}
}
