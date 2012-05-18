using System;
using System.Diagnostics;
using System.Windows;
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
		private readonly object _imageSource;
		private bool _isUploading;
		private bool _isFinished;
		private long _progressCurrent;
		private Guid _currentCorrelation;
		private long _progressTotal;

		public PasteViewModel(ClipboardItem clipboardItem, IRoom room, IMessageBus bus)
		{
			if (clipboardItem.IsImage)
			{
				_imageSource = clipboardItem.LocalPath;
			}
			else
			{
				_imageSource = DependencyProperty.UnsetValue;
			}
			

			Caption = "Upload this " + (clipboardItem.IsImage ? "image" : "file") + "?";
			LocalPath = clipboardItem.LocalPath;
			ShowLocalPath = !clipboardItem.IsImage;
			ContentType = clipboardItem.ContentType;
			Size = clipboardItem.Size;

			PasteCommand = new ReactiveCommand();

			bus.Listen<FileUploadedMessage>().Where(msg => msg.CorrelationId == _currentCorrelation)
				.SubscribeUI(_ => IsFinished = true);

			bus.Listen<FileUploadProgressChangedMessage>().Where(msg => msg.CorrelationId == _currentCorrelation)
				.SubscribeUI(msg =>
					{
						ProgressCurrent = msg.Progress.Current;
						ProgressTotal = msg.Progress.Total;
						Debug.WriteLine("Current, total" + ProgressCurrent + ", " + ProgressTotal);
					});

			ProgressCurrent = 0;
			ProgressTotal = 100;

			bus.RegisterMessageSource(
				PasteCommand.Do(_ => IsUploading = true)
				.Select(_ => new RequestUploadFileMessage(room.Id, clipboardItem.LocalPath, clipboardItem.ContentType))
				.Do(msg => _currentCorrelation = msg.CorrelationId));

			CancelCommand = new ReactiveCommand();
			CancelCommand.Subscribe(_ => IsFinished = true);
		}

		public bool ShowLocalPath { get; private set; }
		public string LocalPath { get; private set; }
		public string Caption { get; private set; }
		public string ContentType { get; private set; }
		public long Size { get; private set; }

		public ReactiveCommand PasteCommand { get; private set; }
		public ReactiveCommand CancelCommand { get; private set; }

		public object ImageSource
		{
			get { return _imageSource; }
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

		public long ProgressCurrent
		{
			get { return _progressCurrent; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ProgressCurrent, ref _progressCurrent, value); }
		}

		public long ProgressTotal
		{
			get { return _progressTotal; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ProgressTotal, ref _progressTotal, value); }
		}

	}
}
