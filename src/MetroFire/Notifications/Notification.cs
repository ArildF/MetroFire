using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class Notification : INotification
	{
		private readonly NotificationEntry _notificationEntry;
		private readonly Func<NotificationAction, INotificationAction> _actionFactory;
		private readonly List<Trigger> _triggers = new List<Trigger>();
		private readonly List<INotificationAction> _actions = new List<INotificationAction>();

		public Notification(NotificationEntry notificationEntry, Func<NotificationAction, INotificationAction> actionFactory)
		{
			_notificationEntry = notificationEntry;
			_actionFactory = actionFactory;

			foreach (var notificationTrigger in notificationEntry.Triggers)
			{
				_triggers.Add(new Trigger(notificationTrigger));
			}
			foreach (var notificationAction in _notificationEntry.Actions)
			{
				var action = _actionFactory(notificationAction);
				_actions.Add(action);
			}
		}

		public void Process(NotificationMessage notificationMessage)
		{
			if (_triggers.Any(t => t.Matches(notificationMessage)))
			{
				ExecuteNotifications(notificationMessage);
			}
		}

		private void ExecuteNotifications(NotificationMessage notificationMessage)
		{
			foreach (var action in _actions)
			{
				action.Execute(notificationMessage);
			}
		}
	}
}
