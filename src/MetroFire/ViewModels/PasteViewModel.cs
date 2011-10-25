using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive.Linq;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class PasteViewModel : ReactiveObject, IPasteViewModel
	{
		private readonly BitmapSource _bitmapSource;
		private bool _isUploading;
		private bool _isFinished;
		private long _progressCurrent;
		private Guid _currentCorrelation;
		private long _progressTotal;

		public PasteViewModel(BitmapSource bitmapSource, IRoom room, IMessageBus bus, IImageEncoder encoder)
		{
			_bitmapSource = bitmapSource;

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
				.Select(_ => encoder.EncodeToTempPng(bitmapSource))
				.Select(path => new RequestUploadFileMessage(room.Id, path, "image/png"))
				.Do(msg => _currentCorrelation = msg.CorrelationId));
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
