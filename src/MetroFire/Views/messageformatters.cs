using System;
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
			string[] results = ChatDocument.UrlDetector.Split(message.Body.Trim()).Where(r => !String.IsNullOrEmpty(r)).ToArray();
			if (results.Length == 1)
			{
				if (ChatDocument.UrlDetector.IsMatch(results.First()))
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

		public void Render(Paragraph paragraph, Message message, User user)
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

		public void Render(Paragraph paragraph, Message message, User user)
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

		public void Process(Paragraph paragraph, Message message, User user)
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

			var prev = run.PreviousInline;
			var span = new Span();

			inlines.Remove(run);
			if (prev != null)
			{
				inlines.InsertAfter(prev, span);
			}
			else if (inlines.FirstInline != null)
			{
				inlines.InsertBefore(inlines.FirstInline, span);
			}
			else
			{
				inlines.Add(span);
			}

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
}
