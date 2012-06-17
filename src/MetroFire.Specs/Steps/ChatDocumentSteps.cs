using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Moq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Views;
using TechTalk.SpecFlow;
using System.Linq;
using FluentAssertions;

namespace MetroFire.Specs.Steps
{
	[Binding]
	[Scope(Feature = "Chat document")]
	public class ChatDocumentSteps
	{
		private readonly ChatDocument _chatDocument;
		private Mock<IMessageBus> _bus;
		private User _user;

		public ChatDocumentSteps()
		{
			_bus = new Mock<IMessageBus>();
			_chatDocument = new ChatDocument(new Mock<IInlineUploadViewFactory>().Object, new Mock<IWebBrowser>().Object, 
				new Mock<IPasteViewFactory>().Object);
		}

		[Given(@"a user '(.*)'")]
		public void GivenAUserTestuser(string name)
		{
			_user = new User {Admin = false, Name = name};
		}

		[When(@"I add the message ""(.*)"" from user '(.*)'")]
		public void WhenIAddTheMessageHelloWorldFromUserTestuser(string message, string user)
		{
			_chatDocument.AddMessage(new Message {Body = message, Type = MessageType.TextMessage}, _user, new object());
		}

		[When(@"user 'Testuser' joins the room")]
		public void WhenUserTestuserJoinsTheRoom()
		{
			_chatDocument.AddMessage(new Message {Type = MessageType.EnterMessage}, _user, new object());
		}

		[When(@"user 'Testuser' leaves the room")]
		public void WhenUserTestuserLeavesTheRoom()
		{
			_chatDocument.AddMessage(new Message {Type = MessageType.LeaveMessage}, _user, new object());
		}

		[When(@"user 'Testuser' is kicked from the room")]
		public void WhenUserTestuserIsKickedFromTheRoom()
		{
			_chatDocument.AddMessage(new Message {Type = MessageType.KickMessage}, _user, new object());
		}


		[Then(@"the message should be displayed like ""(.*)""")]
		public void ThenTheMessageShouldBeDisplayedLikeTestuserHelloWorld(string message)
		{
			var block = (Paragraph)_chatDocument.Blocks.FirstBlock;
			var msg = string.Join("", block.Inlines.GetText());
			msg.Should().Be(message);
		}

		[Then(@"the message should be displayed in italics")]
		public void ThenTheMessageShouldBeDisplayedInItalics()
		{
			var block = (Paragraph) _chatDocument.Blocks.FirstBlock;
			var isItalic = block.Inlines.OfType<Run>().All(r => r.FontStyle == FontStyles.Italic);
			isItalic.Should().BeTrue();
		}

		[Then(@"""(.*)"" should be a hyperlink")]
		public void ThenHttpWww_Test_ComShouldBeAHyperlink(string url)
		{
			var block = (Paragraph) _chatDocument.Blocks.FirstBlock;
			var inline = block.Inlines.Flatten().OfType<Run>().First(r => r.Text == url);
			inline.Parent.Should().BeOfType<Hyperlink>();
		}



	}

	public static class Extensions
	{
		private static readonly Dictionary<Type, Func<Inline, string>> Extractors = new Dictionary<Type, Func<Inline, string>>
			{
				{typeof(Run), i => ((Run)i).Text}
			};

		public static string GetText(this InlineCollection inlines)
		{
			var sb = new StringBuilder();

			foreach (var inline in inlines.Flatten())
			{
				Func<Inline, string> func;
				if (Extractors.TryGetValue(inline.GetType(), out func))
				{
					sb.Append(func(inline));
				}
			}
			return sb.ToString();
		}

		public static IEnumerable<Inline> Flatten(this InlineCollection inlines)
		{
			foreach (var inline in inlines)
			{
				if (inline is Span)
				{
					foreach (var inner in ((Span)inline).Inlines.Flatten())
					{
						yield return inner;
					}
				}
				else
				{
					yield return inline;
				}
			}
		}
	}
}
