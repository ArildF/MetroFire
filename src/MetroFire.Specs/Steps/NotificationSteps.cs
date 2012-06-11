using System;
using System.Linq;
using System.Windows.Documents;
using Moq;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Settings;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace MetroFire.Specs.Steps
{
	[Binding]
	public class NotificationSteps
	{
		private readonly RoomContext _context;

		public NotificationSteps(RoomContext context)
		{
			_context = context;
		}

		[Given(@"that my settings are set to flash taskbar on any message")]
		public void GivenThatMySettingsAreSetToFlashTaskbarOnAnyMessage()
		{
			var settings = new MetroFireSettings()
				{
					General = new GeneralSettings(),
					Network = new NetworkSettings(),
					Notification = new NotificationSettings()
						{
							Notifications = new[]
								{
									new NotificationEntry
										{
											Actions = new NotificationAction[]
												{
													new FlashTaskBarNotificationAction(),
												},
											Triggers = new[]
												{
													new NotificationTrigger {TriggerType = TriggerType.UserMessage},
												}
										}
								}
						}
				};
			_context.ChangeSettings(settings);
		}

		[Given(@"that my settings are set to show a toast on any message")]
		public void GivenThatMySettingsAreSetToShowAToastOnAnyMessage()
		{
			_context.MetroFireSettings.Notification.Notifications = new[]
				{
					new NotificationEntry
						{
							Actions = new NotificationAction[]
								{
									new ShowToastNotificationAction(), 
								},
							Triggers = new[]
								{
									new NotificationTrigger {TriggerType = TriggerType.UserMessage},
								}
						}
				};

			_context.PulseSettingsChanged();
		}

		[When(@"I close the toast by clicking on the X")]
		public void WhenICloseTheToastByClickingOnTheX()
		{
			_context.ToastWindowViewModel.Toasts.First().CloseCommand.Execute(null);
		}

		[When(@"I click on the toast")]
		public void WhenIClickOnTheToast()
		{
			_context.ToastWindowViewModel.Toasts.First().ActivateCommand.Execute(null);
		}




		[Then(@"the taskbar should flash")]
		public void ThenTheTaskbarShouldFlash()
		{
			_context.FlashTaskBarActionMock.Verify(a => a.Execute(It.IsAny<NotificationMessage>()));
		}

		[Then(@"the application should be active")]
		public void ThenTheApplicationShouldBeActive()
		{
			_context.ApplicationActivatorMock.Verify(a => a.Activate());
		}

		[Then(@"a toast should appear containing the words ""(.*)""")]
		public void ThenAToastShouldAppearContainingTheWordsHelloWorld(string text)
		{
			_context.ToastWindowViewModel.Toasts.Should().Contain(
				p => ((ChatViewFake) p.Document).Messages.First().Body == text);

			
		}

		[Then(@"after (\d+) seconds there should be (\d+) toasts")]
		public void ThenAfter6SecondsThereShouldBe0Toasts(int numSeconds, int numToasts)
		{
			Events.TestScheduler.AdvanceBy(TimeSpan.FromSeconds(numSeconds).Ticks);

			_context.ToastWindowViewModel.Toasts.Should().HaveCount(numToasts);
		}

		[Then(@"there should be (\d+) toasts")]
		public void ThenThereShouldBe0Toasts(int numToasts)
		{
			_context.ToastWindowViewModel.Toasts.Should().HaveCount(numToasts);
		}




		[Then(@"the taskbar should not flash")]
		public void ThenTheTaskbarShouldNotFlash()
		{
			_context.FlashTaskBarActionMock.Verify(a => a.Execute(It.IsAny<NotificationMessage>()), Times.Never());
		}

		[Then(@"room ""(.*)"" should be active")]
		public void ThenRoomTestShouldBeActive(string room)
		{
			_context.MainViewModel.ActiveModule.Caption.Should().Be(room);
		}

	}
}
