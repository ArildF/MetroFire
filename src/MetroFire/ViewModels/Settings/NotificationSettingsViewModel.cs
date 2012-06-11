using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;
using System.Linq;
using System;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class NotificationSettingsViewModel : ISettingsSubPage
	{
		private readonly NotificationSettings _notification;

		public NotificationSettingsViewModel(NotificationSettings notification)
		{
			_notification = notification;

			Notifications = new ReactiveCollection<NotificationViewModel>();

			foreach (var notificationEntry in notification.Notifications)
			{
				Notifications.Add(new NotificationViewModel(notificationEntry));
			}

			AddNotificationCommand = new ReactiveCommand();
			AddNotificationCommand.Subscribe(_ => Notifications.Add(new NotificationViewModel(
				new NotificationEntry{Triggers = new NotificationTrigger[]{}, Actions = new NotificationAction[]{}})));
		}

		public ReactiveCommand AddNotificationCommand { get; private set; }

		public ReactiveCollection<NotificationViewModel> Notifications { get; private set; }

		public string Title
		{
			get { return "Notifications"; }
		}

		public void Commit()
		{
			foreach (var notificationViewModel in Notifications)
			{
				notificationViewModel.Commit();
			}
			_notification.Notifications = Notifications.Select(n => n.Notification).ToArray();
		}
	}

}