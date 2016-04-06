using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.ViewModels;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.Views
{

	public class SingleLinkMessageFormatter 
	{
		public virtual bool ShouldHandle(Message message, User user)
		{
			if (message.Type != MessageType.TextMessage)
			{
				return false;
			}
			return GetSingleUrl(message) != null;
		}

		private static string GetSingleUrl(Message message)
		{
			string[] results = UrlDetector.Split(message.Body.Trim()).Where(r => !String.IsNullOrEmpty(r)).ToArray();
			if (results.Length == 1)
			{
				if (UrlDetector.IsMatch(results.First()))
				{
					return results.First();
				}
			}
			return null;
		}
	}
	public class PotentialImageUrlMessageFormatter : SingleLinkMessageFormatter, IMessageFormatter
	{
		private readonly IMessageBus _bus;
		private readonly IInlineUploadViewFactory _factory;

		public PotentialImageUrlMessageFormatter(IMessageBus bus, IInlineUploadViewFactory factory)
		{
			_bus = bus;
			_factory = factory;
		}

		public void Render(Paragraph paragraph, Message message, User user, IRoom room)
		{
			ChatDocument.RenderUserString(user, paragraph);

			var span = new Span();
			var link = ChatDocument.CreateHyperLink(message.Body);
			span.Inlines.Add(link);

			var msg = new RequestHeadMessage(message.Body);
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

			paragraph.Inlines.Add(span);
		}

		public int Priority { get { return 1000; } }
	}

	public abstract class YoutubeUrlHandlerBase : SingleLinkMessageFormatter, IMessageFormatter
	{
		public override bool ShouldHandle(Message message, User user)
		{
			return base.ShouldHandle(message, user) && GetId(message.Body) != null;
		}

		public void Render(Paragraph paragraph, Message message, User user, IRoom room)
		{
			ChatDocument.RenderUserString(user, paragraph);

			var id = GetId(message.Body);

			var span = new Span();
			span.Inlines.Add(ChatDocument.CreateHyperLink(message.Body));
			span.Inlines.Add(Environment.NewLine);

			var vm = new InlineYoutubeViewModel(id);

			var container = new InlineUIContainer(
				new GenericInlineContainer{DataContext = vm});
			span.Inlines.Add(container);

			paragraph.Inlines.Add(span);
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


	public class TriggerFormatter : IMessagePostProcessor
	{
		private readonly IFormatter _formatter;

		public TriggerFormatter(IFormatter formatter)
		{
			_formatter = formatter;
		}

		public bool ShouldHandle(Message message, User user)
		{
			return true;
		}

		public void Process(Paragraph paragraph, Message message, User user, IRoom room)
		{
			_formatter.Format(new NotificationMessage(room, user, message, false),
				new MessageRenderer(paragraph));
		}

		public int Priority { get { return 9999; } }
	}

	public class MessageRenderer : IMessageRenderer
	{
		private readonly Paragraph _paragraph;

		public MessageRenderer(Paragraph paragraph)
		{
			_paragraph = paragraph;
		}

		public void Highlight(Color color)
		{
			new Span(_paragraph.ContentStart, _paragraph.ContentEnd)
			{
				Foreground = new SolidColorBrush(color)
			};
		}
	}

	public class InlineVisitor
	{
		public void Visit(InlineCollection inlines)
		{
			foreach (dynamic inline in inlines.ToArray())
			{
				Visit(inline, inlines);
			}
		}

		protected virtual void Visit(Run run, InlineCollection inlines)
		{
			
		}

		protected virtual void Visit(Hyperlink link, InlineCollection inlines)
		{
			Visit(link as dynamic, inlines);
		}

		protected virtual void Visit(Span span, InlineCollection inlines)
		{
			VisitSpan(span);
		}

		protected virtual void Visit(LineBreak lineBreak, InlineCollection inlines)
		{
		}

		private void VisitSpan(Span span)
		{
			foreach (dynamic inline in span.Inlines.ToArray())
			{
				Visit(inline, span.Inlines);
			}
		}

		protected virtual void Visit(InlineUIContainer container, InlineCollection inlines)
		{
			
		}
	}


	public class EmoticonMessageFormatter : IMessagePostProcessor
	{
		private readonly ResourceDictionary _resources;
		private static readonly string[] Emoticons = new[]
			{
				":-)",
				":)",
				";-)",
				";)",
				":(",
				":-(",
				":D",
				":-D",
				":S",
				":-S",
				":O",
				":-O",
				"'-(",
				"'(",
				">:)",
				">:D",
				":-P",
				":P",
				"(Y)",
				"(N)",
			};
		private readonly Regex _regex;

		public EmoticonMessageFormatter(ResourceDictionary resources)
		{
			_resources = resources;

			_regex = new Regex(String.Join("|", Emoticons.Select(Escape).Select(s => "(" + s + ")")),
				RegexOptions.IgnoreCase);
		}

		private string Escape(string arg)
		{
			return arg.Replace("(", @"\(").Replace(")", @"\)").Replace("-", @"\-")
			          .Replace("|", @"\|");
		}

		public void Process(Paragraph paragraph, Message message, User user, IRoom room)
		{
			if (message.Type != MessageType.TextMessage)
			{
				return;
			}

			var runs = paragraph.Inlines.FlattenWithParents().Where(t => t.Item2 is Run).ToList();
			foreach (var run in runs)
			{
				Emotify(((Run)run.Item2), run.Item1);
			}
		}

		private void Emotify(Run run, InlineCollection inlines)
		{
			if (!_regex.IsMatch(run.Text))
			{
				return;
			}

			var splits = _regex.Split(run.Text);

			var span = inlines.ReplaceInlineWithSpan(run);

			foreach(var result in splits)
			{
				if (_regex.IsMatch(result))
				{
					var style = _resources["Emoticon " + result.ToUpperInvariant()] as Style;
					if (style == null)
					{
						throw new InvalidOperationException("Missing emoticon");
					}

					span.Inlines.Add(new Border
					{
						Style = style,
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Stretch,
						Height = 20,
						Width = 20,
						RenderTransform = new TranslateTransform(0, 1),
						ToolTip = result,
					});
				}
				else if (result != String.Empty)
				{
					span.Inlines.Add(result);
				}
			}
		}

		public int Priority { get { return 10; } }
	}

	public class HyperLinkMessagePostProcessor : IMessagePostProcessor
	{
		public void Process(Paragraph paragraph, Message message, User user, IRoom room)
		{
			new HyperLinkVisitor().Visit(paragraph.Inlines);
		}


		public int Priority { get { return 7; } }

		
	}

	public class EmojiMessageFormatter : IMessagePostProcessor
	{
		
		private readonly Regex _regex;
		private readonly Dictionary<string, EmojiAsset> _emoji;

		public EmojiMessageFormatter(IEmojiProvider provider)
		{
			_emoji = provider.GetEmojis().ToDictionary(e => e.Emoji.Shortname);
			_regex = new Regex(@"(\:[\w_]+\:)");
		}

		public void Process(Paragraph paragraph, Message message, User user, IRoom room)
		{
			if (message.Type != MessageType.TextMessage)
			{
				return;
			}

			var runs = paragraph.Inlines.FlattenWithParents().Where(t => t.Item2 is Run).ToList();
			foreach (var run in runs)
			{
				Emojify(((Run)run.Item2), run.Item1);
			}
		}

		private void Emojify(Run run, InlineCollection inlines)
		{
			if (!_regex.IsMatch(run.Text))
			{
				return;
			}

			var splits = _regex.Split(run.Text);

			var span = inlines.ReplaceInlineWithSpan(run);

			foreach (var result in splits)
			{
				if (_regex.IsMatch(result) && _emoji.ContainsKey(result))
				{
					var asset = _emoji[result];

					span.Inlines.Add(new Image
					{
						Source = new DrawingImage{Drawing = asset.DrawingGroup},
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Stretch,
						Height = 40,
						Width = 40,
						RenderTransform = new TranslateTransform(0, 1),
						ToolTip = result,
					});
				}
				else if (result != String.Empty)
				{
					span.Inlines.Add(result);
				}
			}
		}

		public int Priority { get { return 6; } }
	}


	public class HyperLinkVisitor : InlineVisitor
	{
		protected override void Visit(Hyperlink link, InlineCollection inlines)
		{
			// we don't want to go into the children of existing hyperlinks
		}

		protected override void Visit(Run run, InlineCollection inlines)
		{
			Linkify(run, inlines);
		}

		private void Linkify(Run run, InlineCollection inlines)
		{
			if (!UrlDetector.IsMatch(run.Text))
			{
				return;
			}

			var results = UrlDetector.Split(run.Text);

			var span = inlines.ReplaceInlineWithSpan(run);
			foreach (var result in results)
			{
				if (UrlDetector.IsMatch(result))
				{
					span.Inlines.Add(ChatDocument.CreateHyperLink(result));
				}
				else if (result != String.Empty)
				{
					span.Inlines.Add(result);
				}
			}
		}
	}
}
