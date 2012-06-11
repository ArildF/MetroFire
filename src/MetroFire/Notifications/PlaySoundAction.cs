using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class PlaySoundAction : INotificationAction
	{
		private PlaySoundNotificationAction _data;

		public PlaySoundAction(PlaySoundNotificationAction data)
		{
			_data = data;
		}

		public void Execute(NotificationMessage notificationMessage)
		{
			
		}
	}
}
