using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class ShowToastAction : INotificationAction
	{
		private readonly ShowToastNotificationAction _data;
		private readonly IMessageBus _bus;

		public ShowToastAction(ShowToastNotificationAction data, IMessageBus bus)
		{ 
			_data = data;
			_bus = bus;
		}

		public void Execute(NotificationMessage notificationMessage)
		{
			_bus.SendMessage(new ShowToastMessage(notificationMessage, _data));
		}

		public bool ShouldTriggerOnSelfMessage { get { return false; } }
		public bool IsRenderTime { get { return false; } }
	}
}
