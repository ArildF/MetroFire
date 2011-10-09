using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class FlashTaskBarAction : INotificationAction
	{
		private NotificationAction _data;
		private readonly ITaskBar _taskbar;

		public FlashTaskBarAction(NotificationAction data, ITaskBar taskbar)
		{
			_data = data;
			_taskbar = taskbar;
		}

		public void Execute(NotificationMessage notificationMessage)
		{
			_taskbar.Flash();
		}
	}
}
