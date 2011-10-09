using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class NotificationViewModel
	{
		private readonly NotificationEntry _notificationEntry;

		public NotificationViewModel(NotificationEntry notificationEntry)
		{
			_notificationEntry = notificationEntry;
			Triggers = new ReactiveCollection<TriggerViewModel>();

			foreach (var trigger in notificationEntry.Triggers)
			{
				Triggers.Add(new TriggerViewModel(trigger));
			}

			Actions = new ReactiveCollection<ActionViewModel>();
			foreach (var notificationAction in notificationEntry.Actions)
			{
				Actions.Add(new ActionViewModel(notificationAction));
			}

			AddNewTriggerCommand = new ReactiveCommand();
			AddNewTriggerCommand.Subscribe(_ => Triggers.Add(new TriggerViewModel(new NotificationTrigger())));


			AddNewActionCommand = new ReactiveCommand();
			AddNewActionCommand.Subscribe(_ => Actions.Add(new ActionViewModel(new FlashTaskBarNotificationAction())));

			DeleteTriggerCommand = new ReactiveCommand();
			DeleteTriggerCommand.Cast<TriggerViewModel>().Subscribe(trigger => Triggers.Remove(trigger));

			DeleteActionCommand = new ReactiveCommand();
			DeleteActionCommand.Cast<ActionViewModel>().Subscribe(action => Actions.Remove(action));
		}

		public ReactiveCommand DeleteActionCommand { get; private set; }

		public ReactiveCommand AddNewActionCommand { get; private set; }

		public ReactiveCollection<ActionViewModel> Actions { get; private set; }

		public ReactiveCommand DeleteTriggerCommand { get; private set; }

		public ReactiveCommand AddNewTriggerCommand { get; private set; }

		public ReactiveCollection<TriggerViewModel> Triggers { get; private set; }

		public NotificationEntry Notification
		{
			get { return _notificationEntry; }
		}

		public void Commit()
		{
			foreach (var triggerViewModel in Triggers)
			{
				triggerViewModel.Commit();
			}
			foreach (var actionViewModel in Actions)
			{
				actionViewModel.Commit();
			}

			_notificationEntry.Triggers = Triggers.Select(t => t.Trigger).ToArray();
			_notificationEntry.Actions = Actions.Select(a => a.Action).ToArray();
		}
	}
}