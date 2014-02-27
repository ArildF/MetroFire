using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class HighlightTextAction : INotificationAction, IRenderAction
	{
		private readonly HighlightTextNotificationAction _data;

		public HighlightTextAction(HighlightTextNotificationAction data)
		{
			_data = data;
		}

		public void Execute(NotificationMessage notificationMessage)
		{
		}

		public bool ShouldTriggerOnSelfMessage { get { return true; } }
		public bool IsRenderTime { get { return true; } }

		public void Format(NotificationMessage message, IMessageRenderer renderer)
		{
			renderer.Highlight(_data.Color);
		}
	}
}
