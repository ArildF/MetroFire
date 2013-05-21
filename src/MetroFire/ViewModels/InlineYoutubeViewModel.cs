using System;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class InlineYoutubeViewModel : ViewModelBase
	{
		private bool _showBrowser;
		private bool _showThumbnail;
		private string _htmlDocument;

		public InlineYoutubeViewModel(string id)
		{
			PreviewImageUri = string.Format("http://img.youtube.com/vi/{0}/2.jpg", id);

			ShowBrowser = false;
			ShowThumbnail = true;

			EmbedHtml = String.Format(@"
<body scroll='no'>
		<iframe width='560' height='315'
				src='http://www.youtube.com/embed/{0}?autoplay=1' frameborder='0' allowfullscreen></iframe>
</body>
", id);
			PlayVideoCommand = new ReactiveCommand();
			PlayVideoCommand.Subscribe(_ =>
				{
					ShowBrowser = true;
					ShowThumbnail = false;
					HtmlDocument = EmbedHtml;
				});

			CollapseCommand = new ReactiveCommand();
			CollapseCommand.Subscribe(_ =>
				{
					ShowBrowser = false;
					ShowThumbnail = true;
					HtmlDocument = "<html/>";
				});
		}

		public ReactiveCommand PlayVideoCommand { get; private set; }

		public ReactiveCommand CollapseCommand { get; private set; }

		private string EmbedHtml
		{
			get; set;
		}

		public string HtmlDocument
		{
			get { return _htmlDocument; }
			private set { this.RaiseAndSetIfChanged(vm => vm.HtmlDocument, ref _htmlDocument, value); }
		}

		public bool ShowBrowser
		{
			get { return _showBrowser; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ShowBrowser, ref _showBrowser, value); }
		}

		public bool ShowThumbnail
		{
			get { return _showThumbnail; }
			private set { this.RaiseAndSetIfChanged(vm => vm.ShowThumbnail, ref _showThumbnail, value); }
		}

		public string PreviewImageUri { get; private set; }
	}
}
