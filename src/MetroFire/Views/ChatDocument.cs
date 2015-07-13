using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ColorCode;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;
using Clipboard = System.Windows.Clipboard;

namespace Rogue.MetroFire.UI.Views
{
	public class ChatDocument : FlowDocument, IChatDocument
	{
		private readonly IInlineUploadViewFactory _factory;
		private readonly IPasteViewFactory _pasteViewFactory;
		private readonly ITweetViewFactory _tweetViewFactory;
		private readonly IEnumerable<IMessageFormatter> _formatters;
		private readonly IEnumerable<IMessagePostProcessor> _postProcessors;
		private readonly Dictionary<MessageType, Action<Message, User, Paragraph>> _handlers;

		public static readonly Regex UrlDetector = new Regex(@"(https?://\S+)");

		private const string SyntaxHighlightSpecifierPrefix = 
			"\u200B\u200D\u200B\u200C\u200B\u200B\u200D\u200B\u200C\u200B\u200C\u200C\u200D\u200D\u200B\u200C\u200D\u200B\u200D\u200B";


		public ChatDocument(IInlineUploadViewFactory factory,
			IPasteViewFactory pasteViewFactory, ITweetViewFactory tweetViewFactory, IEnumerable<IMessageFormatter> formatters, 
			IEnumerable<IMessagePostProcessor> postProcessors)
		{
			_factory = factory;
			_pasteViewFactory = pasteViewFactory;
			_tweetViewFactory = tweetViewFactory;
			_formatters = formatters;
			_postProcessors = postProcessors;
			_handlers = new Dictionary<MessageType, Action<Message, User, Paragraph>>
				{
					{MessageType.TextMessage, FormatUserMessage},
					{MessageType.TimestampMessage, FormatTimestampMessage},
					{MessageType.LeaveMessage, (m, u, p) => FormatUserMetaMessage(m,u, p, "left the room")},
					{MessageType.KickMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "was kicked from the room")},
					{MessageType.PasteMessage, FormatPasteMessage},
					{MessageType.EnterMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "entered the room")},
					{MessageType.UploadMessage, FormatUploadMessage},
					{MessageType.TweetMessage, FormatTweetMessage},
					{MessageType.AdvertisementMessage, FormatAdvertisementMessage},
					{MessageType.TopicChangeMessage, FormatTopicChangeMessage},
					{MessageType.LockMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "locked the room")},
					{MessageType.UnlockMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "unlocked the room")},
					{MessageType.AllowGuestsMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "turned on guest access")},
					{MessageType.DisallowGuestsMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "turned off guest access")},
					{MessageType.ConferenceCreatedMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "started a conference call")},
					{MessageType.ConferenceFinishedMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "ended a conference call")},
					{MessageType.IdleMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "is idle")},
					{MessageType.UnidleMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "is back")},
					{MessageType.SoundMessage, (m, u, p) => FormatUserMetaMessage(m, u, p, "played a sound")},
				};

			FontSize = 14;
			FontFamily = new FontFamily("Segoe UI");
		}


		private void FormatTweetMessage(Message msg, User user, Paragraph paragraph)
		{
			if (msg.Tweet != null)
			{
				var tweetView = _tweetViewFactory.Create(msg.Tweet);

				paragraph.Inlines.Add(FormatUserName(user) + ":" + Environment.NewLine);
				paragraph.Inlines.Add(tweetView.Element);
				return;
			}

			// fallback
			FormatUserMessage(msg, user, paragraph);
		}

		private string StripEntities(string message)
		{
			return HttpUtility.HtmlDecode(message);
		}

		private void FormatUploadMessage(Message msg, User user, Paragraph paragraph)
		{
			var vm = _factory.Create(msg);
			var view = _factory.Create(vm);

			RenderUserString(user, paragraph);
			paragraph.Inlines.Add(new LineBreak());

			paragraph.Inlines.Add(view.Element);
		}

		public object AddMessage(Message message, User user, IRoom room, object textObject)
		{
			var paragraph = new Paragraph
			{
				Margin = new Thickness(0), TextAlignment = TextAlignment.Left, Tag = message
			};
			RenderParagraph(paragraph, message, user, room);

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

		public void UpdateMessage(object textObject, Message message, User user, IRoom room)
		{
			var paragraph = textObject as Paragraph;
			if (paragraph == null)
			{
				throw new InvalidOperationException("Invalid object passed in");
			}
			paragraph.Inlines.Clear();

			RenderParagraph(paragraph, message, user, room);
		}

		private void RenderParagraph(Paragraph paragraph, Message message, User user, IRoom room)
		{
			Action<Message, User, Paragraph> handler;
			if (!_handlers.TryGetValue(message.Type, out handler))
			{
				return;
			}
			if (!RenderByCustomFormatter(message, user, paragraph, room))
			{
				handler(message, user, paragraph);
			}

			
			foreach (var postProcessor in _postProcessors.OrderBy(p => p.Priority))
			{
				postProcessor.Process(paragraph, message, user ?? User.NullUser, room);
			}
		}

		private bool RenderByCustomFormatter(Message message, User user, Paragraph paragraph, IRoom room)
		{
			foreach (var messageFormatter in _formatters.OrderBy(f => f.Priority))
			{
				if (messageFormatter.ShouldHandle(message, user))
				{
					messageFormatter.Render(paragraph, message, user, room);
					return true;
				}
			}

			return false;
		}

		public void RemoveMessage(object textObject)
		{
			var obj = textObject as Paragraph;
			if (obj != null)
			{
				Blocks.Remove(obj);
			}
		}

		public void AddNotificationMessage(string message, bool isError)
		{
			var paragraph = new Paragraph(
				new Run(message)
				{
					Foreground = isError ? Brushes.Red : Brushes.Black
				})
			{
				Margin = new Thickness(0),
				TextAlignment = TextAlignment.Left
			};

			Blocks.Add(paragraph);
		}

		private void FormatAdvertisementMessage(Message message, User user, Paragraph paragraph)
		{
			user = new User {Name = "Advertisement"};
			FormatUserMessage(message, user, paragraph);
		}

		private void FormatTopicChangeMessage(Message message, User user, Paragraph paragraph)
		{
			FormatUserMetaMessage(message, user, paragraph, 
				String.Format("changed the topic to '{0}'", message.Body));
		}

		private static void FormatUserMetaMessage(Message message, User user, Paragraph paragraph, string text)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(CreateMetaRun(name + " " + text));
		}

		

		private void FormatPasteMessage(Message message, User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);
			paragraph.Inlines.Add(name + ": " + Environment.NewLine);

			Inline run;
			if (message.Body.StartsWith(SyntaxHighlightSpecifierPrefix))
			{
				run = FormatWithSyntaxHighlighting(message.Body);

			}
			else
			{
				run = RenderUserMessage(message.Body); 
			}
			run.FontFamily = new FontFamily("Consolas");

			paragraph.BorderThickness = new Thickness(0, 0.5, 0, 0.5);
			paragraph.BorderBrush = Brushes.Black;
			paragraph.Margin = new Thickness(0, 1, 0, 6);
			paragraph.Padding = new Thickness(0, 1, 0, 6);
			paragraph.Background = Brushes.LightGoldenrodYellow;

			var view = _pasteViewFactory.CreateTextPasteView(run);
			paragraph.Inlines.Add(view.Element);
		}

		private Inline FormatWithSyntaxHighlighting(string body)
		{
			var languageString = UnicodeConvert.FromNonPrintableString(body);
			if (languageString.Length <= 4)
			{
				return RenderUserMessage(body);
			}

			var languageDefString = languageString.Substring(4);
			var languageDef = Languages.FindById(languageDefString);
			if (languageDef == null)
			{
				return RenderUserMessage(body);
			}

			var formatter = new WpfFormatter();
			var colorizer = new CodeColorizer();
			colorizer.Colorize(body, languageDef, formatter, StyleSheets.Default, 
				new StreamWriter(Stream.Null));

			return formatter.Inline;

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
			return new Run(body);
		}

		public static Inline CreateHyperLink(string result)
		{
			var hyperlink = new Hyperlink(new Run(result)) {NavigateUri = new Uri(result)};
			ToolTipService.SetToolTip(hyperlink, result);

		    var menu = new ContextMenu();
            var item = new MenuItem
            {
                Header = "Copy link"
            };
		    item.Click += (sender, args) => Clipboard.SetText(result);
		    menu.Items.Add(item);
            hyperlink.ContextMenu = menu;
			return hyperlink;
		}


	    public static void RenderUserString(User user, Paragraph paragraph)
		{
			var name = FormatUserName(user);

			var item = new Run("<" + name + "> ");
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

		public void AddUploadFile(IRoom room, FileItem fileItem)
		{
			var view = _pasteViewFactory.Create(room, fileItem);
			var paragraph = new Paragraph();
			paragraph.Inlines.Add(view.Element);

			Blocks.Add(paragraph);
			view.Closing.Subscribe(_ =>
				{
					Blocks.Remove(paragraph);
					_pasteViewFactory.Release(view);
				});
		}

		public void HighlightMessage(Message message, Color color)
		{
			var paragraph = Blocks.OfType<Paragraph>().FirstOrDefault(b => b.Tag.Equals(message));
			if (paragraph == null)
			{
				return;
			}
			
		}
	}

}
