using System.Collections.Generic;
using Castle.Core;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.Notifications
{
	public class NotificationListener : IStartable, IFormatter
	{
		private readonly IMessageBus _bus;
		private readonly ISettings _settings;
		private readonly Func<NotificationEntry, INotification> _notificationFactory;

		private readonly List<INotification> _notifications = new List<INotification>();

		public NotificationListener(IMessageBus bus, ISettings settings, Func<NotificationEntry, INotification> notificationFactory)
		{
			_bus = bus;
			_settings = settings;
			_notificationFactory = notificationFactory;

			BuildNotifications(settings);
		}

		public void Start()
		{
			_bus.Listen<NotificationMessage>().Subscribe(OnNotificationMessage);
			_bus.Listen<SettingsChangedMessage>().Subscribe(msg => BuildNotifications(_settings));
		}

		private void OnNotificationMessage(NotificationMessage notificationMessage)
		{
			foreach (var notification in _notifications)
			{
				notification.Process(notificationMessage);
			}
		}

		private void BuildNotifications(ISettings settings)
		{
			_notifications.Clear();
			foreach (var notificationEntry in settings.Notification.Notifications)
			{
				var notification = _notificationFactory(notificationEntry);

				_notifications.Add(notification);
			}
		}

		public void Stop()
		{
			
		}

		public void Format(NotificationMessage message, IMessageRenderer renderer)
		{
			foreach (var notification in _notifications)
			{
				notification.OnRender(message, renderer);
			}
		}
	}

}
