using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Moq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI.ViewModels;
using Rogue.MetroFire.UI.Views;
using TechTalk.SpecFlow;
using System.Linq;
using FluentAssertions;
using Castle.Core.Internal;

namespace MetroFire.Specs.Steps
{
	[Binding]
	[Scope(Feature = "Chat document")]
	public class ChatDocumentSteps
	{
		private readonly RoomContext _context;
		private ChatDocument _chatDocument;
		private string _currentRoom;

		public ChatDocumentSteps(RoomContext context)
		{
			_context = context;
		}

		[Given(@"that the current room is ""(.*)""")]
		public void GivenThatTheCurrentRoomIsTest(string roomName)
		{
			_currentRoom = roomName;
			_chatDocument = (ChatDocument) _context.ViewModelFor(roomName).ChatDocument;
		}


		[When(@"I add the message ""(.*)"" from user '(.*)'")]
		public void WhenIAddTheMessageHelloWorldFromUserTestuser(string message, string user)
		{
			_context.SendRoomMessages(_currentRoom, new Message
				{
					Body = message, Type = MessageType.TextMessage, UserIdString = _context.GetUser(user).Id.ToString()
				});
		}
	
		[When(@"user '(.*)' joins the room")]
		public void WhenUserTestuserJoinsTheRoom(string user)
		{
			_context.SendRoomMessages(_currentRoom, new Message
				{
					Type = MessageType.EnterMessage,
					UserIdString = _context.GetUser(user).Id.ToString()
				});
		}

		[When(@"user '(.*)' leaves the room")]
		public void WhenUserTestuserLeavesTheRoom(string user)
		{
			_context.SendRoomMessages(_currentRoom, 
				new Message
					{
						Type = MessageType.LeaveMessage,
						UserIdString = _context.GetUser(user).Id.ToString()
					});
		}

		[When(@"user '(.*)' is kicked from the room")]
		public void WhenUserTestuserIsKickedFromTheRoom(string user)
		{
			_context.SendRoomMessages(_currentRoom, 
				new Message
					{
						Type = MessageType.KickMessage,
						UserIdString = _context.GetUser(user).Id.ToString()
					});
		}

		[When(@"I add (\d+) image pastes to the room")]
		public void WhenIAdd40ImagePastesToTheRoom(int num)
		{
			_context.PutImageOnClipboard();
			for (int i = 0; i < num; i++)
			{
				_context.ViewModelFor(_currentRoom).PasteCommand.Execute(null);
			}
		}

		[When(@"I cancel all the image pastes in the room")]
		public void WhenICancelAllTheImagePastesInTheRoom()
		{
			var vms = _chatDocument.Blocks.OfType<Paragraph>().SelectMany(p => p.Inlines).OfType<InlineUIContainer>()
				.Select(uc => uc.Child).OfType<IPasteView>().Select(pv => pv.Element.DataContext).Cast<PasteViewModel>().ToArray();
			vms.ForEach(vm => vm.CancelCommand.Execute(null));

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

		[Then(@"""(.*)"" should not be a hyperlink")]
		public void ThenShouldNotBeAHyperlink(string url)
		{
			var block = (Paragraph) _chatDocument.Blocks.FirstBlock;
			var inline = block.Inlines.Flatten().OfType<Run>().First(r => r.Text.Contains(url));
			inline.Parent.GetType().Should().NotBe(typeof (Hyperlink));

		}


		[Then(@"there should be (\d+) PasteViewModels in the system")]
		public void ThenThereShouldBe0PasteViewModelsInTheSystem(int num)
		{
			_context.NumComponentsOfType<PasteViewModel>().Should().Be(0);
		}

		[Then(@"the message should be displayed as an inline youtube video")]
		public void ThenTheMessageShouldBeDisplayedAsAnInlineYoutubeVideo()
		{
			var inlines = Flattened();

			var youtube = inlines.FirstOrDefault(i => i is InlineUIContainer);
			youtube.Should().NotBeNull();
			var child = LogicalTreeHelper.GetChildren(youtube).OfType<GenericInlineContainer>().First();
			child.DataContext.Should().BeOfType<InlineYoutubeViewModel>();
		}

		[Then(@"the message '(.*)' should have the '(.*)' replaced by a graphic")]
		public void ThenTheMessageShouldHaveTheReplacedByAGraphic(string msg, string emoticon)
		{
			var inlines = Flattened();

			int index = msg.IndexOf(emoticon);
			var first = inlines.OfType<Run>().Skip(1).First();
			first.Text.Should().Be(msg.Substring(0, index));

			first.NextInline.Should().BeOfType<InlineUIContainer>();
		}

		[Then(@"the message '(.*)' should be displayed rendered in '(.*)'")]
		public void ThenTheMessageShouldBeDisplayedRenderedInRed(string message, string color)
		{
			var first = ((Paragraph)_chatDocument.Blocks.FirstBlock).Inlines.First();
			first.Should().BeOfType<Span>();
			var span = (Span) first;
			var brush = (SolidColorBrush) span.Foreground;
			brush.Color.Should().Be(ColorConverter.ConvertFromString(color));

			var block = (Paragraph)_chatDocument.Blocks.FirstBlock;
			var msg = string.Join("", block.Inlines.GetText());
			msg.Should().Be(message);
		}

		private IEnumerable<Inline> Flattened()
		{
			var block = (Paragraph) _chatDocument.Blocks.FirstBlock;
			var inlines = block.Inlines.Flatten();
			return inlines;
		}
	}

	
}
