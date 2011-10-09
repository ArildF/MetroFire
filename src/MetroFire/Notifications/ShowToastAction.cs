using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class ShowToastAction : INotificationAction
	{
		private ShowToastNotificationAction _data;

		public ShowToastAction(ShowToastNotificationAction data)
		{
			_data = data;
		}

		public void Execute(NotificationMessage notificationMessage)
		{
			
		}
	}
}
