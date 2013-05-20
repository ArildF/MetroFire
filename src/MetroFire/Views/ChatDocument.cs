using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Windows.Themes;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;
using System.Linq;
using System.Reactive.Linq;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI.Views
{
	public class ChatDocument : FlowDocument, IChatDocument
	{
		private readonly IInlineUploadViewFactory _factory;
		private readonly IWebBrowser _browser;
		private readonly IPasteViewFactory _pasteViewFactory;
		private readonly IMessageBus _bus;
		private readonly Dictionary<MessageType, Action<Message, User, Paragraph>> _handlers;

		private static readonly Regex UrlDetector = new
			Regex(@"((?:http|https|ftp)\://(?:[a-zA-Z0-9\.\-]+(?:\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)?(?:(?:25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(?:25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(?:25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(?:25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|(?:[a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(?:\:[0-9]+)?(?:/[^/][a-zA-Z0-9\.\,\?\'\\/\+&amp;%\$#\=~_\-@]*)*)");
			

		private static readonly Regex TwitterMessageParser =
			new Regex(@"@(.*?),\s*(.*)");

		public ChatDocument(IInlineUploadViewFactory factory, IWebBrowser browser, 
			IPasteViewFactory pasteViewFactory,
			IMessageBus bus)
		{
			_factory = factory;
			_browser = browser;
			_pasteViewFactory = pasteViewFactory;
			_bus = bus;
			_handlers = new Dictionary<MessageType, Action<Message, User, Paragraph>>
				{
					{MessageType.TextMessage, FormatUserMessage},
					{MessageType.TimestampMessage, FormatTimestampMessage},
					{MessageType.LeaveMessage, FormatLeaveMessage},
					{MessageType.KickMessage, FormatKickMessage},
					{MessageType.PasteMessage, FormatPasteMessage},
					{MessageType.EnterMessage, FormatEnterMessage},
					{MessageType.UploadMessage, FormatUploadMessage},
					{MessageType.TweetMessage, FormatTweetMessage},
					{MessageType.AdvertisementMessage, FormatAdvertisementMessage},
					{MessageType.TopicChangeMessage, FormatTopicChangeMessage}

				};

			FontSize = 14;
			FontFamily = new FontFamily("Segoe UI");

			AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(NavigateToLink));
		}

		private void FormatTweetMessage(Message msg, User user, Paragraph paragraph)
		{
			if (msg.Tweet != null)
			{
				var message = StripEntities(msg.Tweet.Message);

				var bodyInline = RenderUserMessage(message);

				var tweetView = new InlineTweetView(bodyInline, msg.Tweet);

				paragraph.Inlines.Add(FormatUserName(user) + ":" + Environment.NewLine);
				paragraph.Inlines.Add(tweetView);
				return;
			}

			// fallback
			FormatUserMessage(msg, user, paragraph);
		}

		private string StripEntities(string message)
		{
			return HttpUtility.HtmlDecode(message);
		}

		private void NavigateToLink(object sender, RequestNavigateEventArgs requestNavigateEventArgs)
		{
			_browser.NavigateTo(requestNavigateEventArgs.Uri);
		}

		private void FormatUploadMessage(Message msg, User user, Paragraph paragraph)
		{
			var vm = _factory.Create(msg);
			var view = _factory.Create(vm);

			RenderUserString(user, paragraph);
			paragraph.Inlines.Add(new LineBreak());

			paragraph.Inlines.Add(view.Element);
		}

		public object AddMessage(Message message, User user, object textObject)
		{
			Action<Message, User, Paragraph> handler;
			if (!_handlers.TryGetValue(message.Type, out handler))
			{
				return null;
			}
			var paragraph = new Paragraph {Margin = new Thickness(0)};
			handler(message, user, paragraph);

			var after = textObject as Paragraph;
			if (after != null)
			{
				Blocks.InsertAfter(after, paragraph);
			}
			else
			{
				Blocks.Add(paragraph);
			}

			return paragraph;
		}

		public void RemoveMessage(object textObject)
		{
			var obj = textObject as Paragraph;
			if (obj != null)
			{
				Blocks.Remove(obj);
			}
		}

		private void FormatEnterMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} entered the room", name)));
		}

		private void FormatAdvertisementMessage(Message message, User user, Paragraph paragraph)
		{
			user = new User {Name = "Advertisement"};
			FormatUserMessage(message, user, paragraph);
		}

		private void FormatTopicChangeMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} changed the topic to '{1}'", name, message.Body)));
		}

		private void FormatPasteMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(name + ": " + Environment.NewLine);

			var run = RenderUserMessage(message.Body); 
			run.FontFamily = new FontFamily("Consolas");

			paragraph.BorderThickness = new Thickness(0, 0.5, 0, 0.5);
			paragraph.BorderBrush = Brushes.Black;
			paragraph.Margin = new Thickness(0, 1, 0, 6);
			paragraph.Padding = new Thickness(0, 1, 0, 6);
			paragraph.Background = Brushes.LightGoldenrodYellow;

			var view = _pasteViewFactory.CreateTextPasteView(run);
			paragraph.Inlines.Add(view.Element);
		}


		private void FormatLeaveMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} left the room", name)));
		}

		private void FormatKickMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(String.Format("{0} was kicked from the room", name)));
		}

		private void FormatUserMessage(Message message, User user, Paragraph paragraph)
		{
			if (message.Body.StartsWith("/me", StringComparison.InvariantCultureIgnoreCase))
			{
				paragraph.Inlines.Add(RenderUserMessage(FormatUserName(user) + message.Body.Substring("/me".Length)));

				paragraph.FontStyle = FontStyles.Italic;
			}
			else
			{
				RenderUserString(user, paragraph);
				var inline = RenderUserMessage(message.Body);
				paragraph.Inlines.Add(inline);
			}

			paragraph.BorderThickness = new Thickness(0, 0, 0, 0.2);
			paragraph.BorderBrush = Brushes.LightGray;
		}

		private Inline RenderUserMessage(string body)
		{
			string[] results = UrlDetector.Split(body).Where(r => !String.IsNullOrEmpty(r)).ToArray();
			if (results.Length == 1)
			{
				if (UrlDetector.IsMatch(results.First()))
				{
					return RenderLink(results.First());
				}
				
				return new Run(body);
			}

			var span = new Span();
			foreach (var result in results)
			{
				if (UrlDetector.IsMatch(result))
				{
					span.Inlines.Add(CreateHyperLink(result));
				}
				else
				{
					span.Inlines.Add(result);
				}
			}
			return span;

		}

		private Inline PotentiallyRenderYoutubeVideo(string uriString)
		{
			var uri = new Uri(uriString);

			var id = ParseFullYoutubeUrl(uri) ?? ParseShortYoutubeUrl(uri);
			if (id == null)
			{
				return null;
			}

			var embedHtml = String.Format(@"
<body scroll='no'>
		<iframe width='300' height='150'
				src='http://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>
</body>
", id);

			var span = new Span();
			span.Inlines.Add(CreateHyperLink(uriString));

			var browser = new System.Windows.Controls.WebBrowser {MinHeight = 170};
			ScrollViewer.SetVerticalScrollBarVisibility(browser, ScrollBarVisibility.Hidden);
			browser.NavigateToString(embedHtml);
			span.Inlines.Add(new InlineUIContainer(browser));

			return span;
		}

		private string ParseShortYoutubeUrl(Uri uri)
		{
			if (!uri.Host.EndsWith("youtu.be", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			var path = uri.LocalPath;
			return path.Length > 0 ? path.Substring(1) : null;
		}

		private static string ParseFullYoutubeUrl(Uri uri)
		{
			if (!uri.Host.EndsWith("youtube.com", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			var parameters = HttpUtility.ParseQueryString(uri.Query);
			string id = parameters["v"];
			return id;
		}

		private static Inline CreateHyperLink(string result)
		{
			var hyperlink = new Hyperlink(new Run(result)) {NavigateUri = new Uri(result)};
			ToolTipService.SetToolTip(hyperlink, result);
			return hyperlink;
		}

		private Inline RenderLink(string uri)
		{
			var inlineYoutubeVideo = PotentiallyRenderYoutubeVideo(uri);
			if (inlineYoutubeVideo != null)
			{
				return inlineYoutubeVideo;
			}

			var span = new Span();
			var link = CreateHyperLink(uri);
			span.Inlines.Add(link);

			var msg = new RequestHeadMessage(uri);
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

		private static void RenderUserString(User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);

			var item = new Run("<" + name + "> "){};
			paragraph.Inlines.Add(item);
		}

		private static string FormatUserName(User user)
		{
			var name = user != null ? user.Name : "<unknown>";
			return name;
		}

		private void FormatTimestampMessage(Message message, User user, Paragraph paragraph)
		{
			var run = CreateMetaRun(message.CreatedAt.ToLocalTime().ToString());
			paragraph.Inlines.Add(run);
		}

		private static Run CreateMetaRun(string message)
		{
			return new Run(message) {FontStyle = FontStyles.Italic, Foreground = Brushes.Gray};
		}

		public void UpdateMessage(object textObject, Message message, User user)
		{
			var paragraph = textObject as Paragraph;
			if (paragraph == null)
			{
				throw new InvalidOperationException("Invalid object passed in");
			}

			Action<Message, User, Paragraph> handler;
			if (!_handlers.TryGetValue(message.Type, out handler))
			{
				return;
			}
			paragraph.Inlines.Clear(); 
			handler(message, user, paragraph);
		}

		public void AddPasteFile(IRoom room, ClipboardItem clipboardItem)
		{
			var view = _pasteViewFactory.Create(room, clipboardItem);
			var paragraph = new Paragraph();
			paragraph.Inlines.Add(view.Element);

			Blocks.Add(paragraph);
			view.Closing.Subscribe(_ =>
				{
					Blocks.Remove(paragraph);
					_pasteViewFactory.Release(view);
				});
		}
	}

}
