using System.Diagnostics;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;
using System.Linq;
using System;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public interface IToggleEdit
	{
		bool IsEditing { get; set; }
	}

	public class NotificationSettingsViewModel : ISettingsSubPage
	{
		private readonly NotificationSettings _notification;

		public NotificationSettingsViewModel(NotificationSettings notification)
		{
			_notification = notification;

			Notifications = new ReactiveCollection<NotificationViewModel>();

			foreach (var notificationEntry in notification.Notifications)
			{
				Notifications.Add(new NotificationViewModel(notificationEntry, this));
			}

			AddNotificationCommand = new ReactiveCommand();
			AddNotificationCommand.Subscribe(_ =>
				{
					Notifications.Add(new NotificationViewModel(
					                  	new NotificationEntry
					                  		{
					                  			Triggers = new[] {new NotificationTrigger()},
					                  			Actions = new NotificationAction[]
					                  			{
					                  				new FlashTaskBarNotificationAction()
					                  			}
					                  		},
					                  	this));
					Notifications.Last().Actions.First().IsEditing = true;
					Notifications.Last().Triggers.First().IsEditing = true;
				});


			DeleteNotificationCommand = new ReactiveCommand();
			DeleteNotificationCommand.OfType<NotificationViewModel>().Subscribe(n => Notifications.Remove(n));

			(ToggleEditCommand = new ReactiveCommand()).Do(_ => Trace.WriteLine(_)).OfType<IToggleEdit>().Subscribe(editable =>
			{
				bool valueToSet = !editable.IsEditing;
				foreach (var notificationViewModel in Notifications)
				{
					notificationViewModel.CollapseAll();
				}

				editable.IsEditing = valueToSet;
			});
		}

		public ReactiveCommand ToggleEditCommand { get; private set; }


		public ReactiveCommand AddNotificationCommand { get; private set; }
		public ReactiveCommand DeleteNotificationCommand { get; private set; }

		public ReactiveCollection<NotificationViewModel> Notifications { get; private set; }

		public string Title
		{
			get { return "_Notifications"; }
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