using Moq;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Settings;
using TechTalk.SpecFlow;

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

		[Then(@"the taskbar should flash")]
		public void ThenTheTaskbarShouldFlash()
		{
			_context.FlashTaskBarActionMock.Verify(a => a.Execute(It.IsAny<NotificationMessage>()));
		}
	}
}
