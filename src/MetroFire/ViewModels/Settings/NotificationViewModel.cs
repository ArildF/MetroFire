using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class NotificationViewModel : ReactiveObject
	{
		private readonly NotificationEntry _notificationEntry;
		private readonly NotificationSettingsViewModel _parent;
		private TriggerViewModel _selectedTrigger;
		private ActionViewModel _selectedAction;

		public NotificationViewModel(NotificationEntry notificationEntry, NotificationSettingsViewModel parent)
		{
			_notificationEntry = notificationEntry;
			_parent = parent;
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
			AddNewTriggerCommand.Subscribe(_ =>
				{
					Triggers.Add(new TriggerViewModel(new NotificationTrigger()));
					SelectedTrigger = Triggers.Last();
					_parent.ToggleEditCommand.Execute(Triggers.Last());
				});


			AddNewActionCommand = new ReactiveCommand();
			AddNewActionCommand.Subscribe(_ =>
				{
					Actions.Add(new ActionViewModel(new FlashTaskBarNotificationAction()));
					SelectedAction = Actions.Last();
					_parent.ToggleEditCommand.Execute(Actions.Last());
				});

			DeleteItemCommand = new ReactiveCommand();
			DeleteItemCommand.OfType<TriggerViewModel>().Subscribe(t => Triggers.Remove(t));
			DeleteItemCommand.OfType<ActionViewModel>().Subscribe(a => Actions.Remove(a));
		}

		public ReactiveCommand AddNewActionCommand { get; private set; }

		public ReactiveCollection<ActionViewModel> Actions { get; private set; }

		public ReactiveCommand DeleteItemCommand { get; private set; }

		public ReactiveCommand AddNewTriggerCommand { get; private set; }

		public ReactiveCollection<TriggerViewModel> Triggers { get; private set; }

		public NotificationEntry Notification
		{
			get { return _notificationEntry; }
		}

		public TriggerViewModel SelectedTrigger
		{
			get { return _selectedTrigger; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedTrigger, ref _selectedTrigger, value);}
		}

		public ActionViewModel SelectedAction
		{
			get { return _selectedAction; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedAction, ref _selectedAction, value); }
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

		public void CollapseAll()
		{
			foreach (var editable in Triggers.Cast<IToggleEdit>().Concat(Actions))
			{
				editable.IsEditing = false;
			}
		}
	}
}