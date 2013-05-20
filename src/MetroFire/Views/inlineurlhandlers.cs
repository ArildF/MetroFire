using System;
using System.Reactive.Linq;
using System.Web;
using System.Windows.Controls;
using System.Windows.Documents;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Views
{
	public class PotentialImageUrlHandler : IInlineUrlHandler
	{
		private readonly IMessageBus _bus;
		private readonly IInlineUploadViewFactory _factory;

		public PotentialImageUrlHandler(IMessageBus bus, IInlineUploadViewFactory factory)
		{
			_bus = bus;
			_factory = factory;
		}

		public bool CanHandle(string url)
		{
			return true;
		}

		public Inline Render(string url)
		{
			var span = new Span();
			var link = ChatDocument.CreateHyperLink(url);
			span.Inlines.Add(link);

			var msg = new RequestHeadMessage(url);
			_bus.Listen<RequestHeadReplyMessage>().Where(m => m.CorrelationId == msg.CorrelationId)
				.Where(m => m.Info.IsOk && m.Info.MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
				.Subscribe(m =>
					{
						var vm = _factory.Create(m.Info);
						var view = _factory.Create(vm);

						var lb = new LineBreak();

						span.Inlines.InsertBefore(link, lb);
						var newItem = new InlineUIContainer(view.Element);
						span.Inlines.InsertBefore(lb, newItem);
						span.Inlines.InsertBefore(newItem, new LineBreak());
					});

			_bus.SendMessage(msg);

			return span;
		}

		public int Priority { get { return 1000; } }
	}

	public abstract class YoutubeUrlHandlerBase : IInlineUrlHandler
	{
		public bool CanHandle(string url)
		{
			var id = GetId(url);
			return id != null;
		}

		public Inline Render(string url)
		{
			var id = GetId(url);

			var embedHtml = String.Format(@"
<body scroll='no'>
		<iframe width='300' height='150'
				src='http://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>
</body>
", id);

			var span = new Span();
			span.Inlines.Add(ChatDocument.CreateHyperLink(url));

			var browser = new System.Windows.Controls.WebBrowser { MinHeight = 170 };
			ScrollViewer.SetVerticalScrollBarVisibility(browser, ScrollBarVisibility.Hidden);
			browser.NavigateToString(embedHtml);
			span.Inlines.Add(new InlineUIContainer(browser));

			return span;
		}

		protected abstract string GetId(string url);
		
		public int Priority 
		{
			get { return 50; }
		}
	}

	public class FullYoutubeUrlHandler : YoutubeUrlHandlerBase
	{
		protected override string GetId(string url)
		{
			var uri = new Uri(url);
			if (!uri.Host.EndsWith("youtube.com", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			var parameters = HttpUtility.ParseQueryString(uri.Query);
			string id = parameters["v"];
			return id;
		}
	}

	public class ShortYoutubeUrlHandler : YoutubeUrlHandlerBase
	{
		protected override string GetId(string url)
		{
			var uri = new Uri(url);

			if (!uri.Host.EndsWith("youtu.be", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			var path = uri.LocalPath;
			return path.Length > 0 ? path.Substring(1) : null;
		}
	}
}
