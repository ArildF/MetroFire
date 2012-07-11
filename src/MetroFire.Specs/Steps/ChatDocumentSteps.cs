using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Moq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
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

		[Then(@"there should be (\d+) PasteViewModels in the system")]
		public void ThenThereShouldBe0PasteViewModelsInTheSystem(int num)
		{
			_context.NumComponentsOfType<PasteViewModel>().Should().Be(0);
		}




	}

	public static class Extensions
	{
		private static readonly Dictionary<Type, Func<Inline, string>> Extractors = new Dictionary<Type, Func<Inline, string>>
			{
				{typeof(Run), i => ((Run)i).Text}
			};

		public static string GetText(this IEnumerable<Inline> inlines)
		{
			var sb = new StringBuilder();

			foreach (var inline in inlines.Flatten())
			{
				var s = inline.GetText();
				if (s != null)
				{
					sb.Append(s);
				}
			}
			return sb.ToString();
		}

		public static string GetText(this Paragraph paragraph)
		{
			return paragraph.Inlines.GetText();
		}

		public static string GetText(this Inline inline)
		{
			Func<Inline, string> func;
			if (Extractors.TryGetValue(inline.GetType(), out func))
			{
				return func(inline);
			}
			return null;
		}

		public static IEnumerable<Inline> Flatten(this IEnumerable<Inline> inlines)
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
