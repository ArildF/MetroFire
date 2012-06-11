using System.Linq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public interface IActionSubViewModel
	{
		void Commit();
	}
	public class ActionViewModel : ReactiveObject
	{
		private NotificationAction _notificationAction;
		private ComboViewModel<ActionType> _selectedActionType;
		private ComboViewModel<ActionCondition> _selectedActionCondition;
		private int _interval;
		private bool _showInterval;
		private string _intervalText;
		private IActionSubViewModel _subViewModel;

		public ActionViewModel(NotificationAction notificationAction)
		{
			_notificationAction = notificationAction;

			ActionTypes = new[]
				{
					new ComboViewModel<ActionType>("flash the task bar", ActionType.FlashTaskbar),
					new ComboViewModel<ActionType>("show a toast", ActionType.ShowToast),
					new ComboViewModel<ActionType>("play a sound", ActionType.PlaySound),
				};

			ActionConditions = new[]
				{
					new ComboViewModel<ActionCondition>("always", ActionCondition.None),
					new ComboViewModel<ActionCondition>("when idle for at least", ActionCondition.RoomIdle),
					new ComboViewModel<ActionCondition>("last notification was at least",
					                                    ActionCondition.MinimumTimeSinceLastNotification),
				};


			SelectedActionType = ActionTypes.FirstOrDefault(at => at.Data == notificationAction.ActionType);
			SelectedActionCondition = ActionConditions.FirstOrDefault(ac => ac.Data == notificationAction.ActionCondition);
			Interval = notificationAction.Interval;
		}

		public ComboViewModel<ActionCondition>[] ActionConditions { get; private set; }


		public ComboViewModel<ActionType> SelectedActionType
		{
			get { return _selectedActionType; }
			set
			{
				if (value == _selectedActionType)
				{
					return;
				}
				this.RaiseAndSetIfChanged(vm => vm.SelectedActionType, ref _selectedActionType, value);

				_notificationAction = CreateNotificationAction(_selectedActionType.Data);
				SubViewModel = CreateSubViewModel(_selectedActionType.Data);
				raisePropertyChanged(null);
			}
		}

		private IActionSubViewModel CreateSubViewModel(ActionType data)
		{
			return data == ActionType.PlaySound ? new PlaySoundViewModel((PlaySoundNotificationAction)_notificationAction) : null;
		}

		public IActionSubViewModel SubViewModel
		{
			get { return _subViewModel; }
			set { this.RaiseAndSetIfChanged(vm => vm.SubViewModel, ref _subViewModel, value); }
		}

		public ComboViewModel<ActionCondition> SelectedActionCondition
		{
			get { return _selectedActionCondition; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.SelectedActionCondition, ref _selectedActionCondition, value);
				ShowInterval = value.Data.In(ActionCondition.MinimumTimeSinceLastNotification, ActionCondition.RoomIdle);
				IntervalText = value.Data == ActionCondition.MinimumTimeSinceLastNotification
				               	? ("seconds ago")
				               	: (value.Data == ActionCondition.RoomIdle ? "seconds" : "");
			}
		}


		public int Interval
		{
			get { return _interval; }
			set { this.RaiseAndSetIfChanged(vm => vm.Interval, ref _interval, value); }
		}

		public string IntervalText
		{
			get { return _intervalText; }
			set { this.RaiseAndSetIfChanged(vm => vm.IntervalText, ref _intervalText, value); }
		}

		public bool ShowInterval
		{
			get { return _showInterval; }
			set { this.RaiseAndSetIfChanged(vm => vm.ShowInterval, ref _showInterval, value); }
		}

		private NotificationAction CreateNotificationAction(ActionType selectedActionType)
		{
			switch (selectedActionType)
			{
				case ActionType.FlashTaskbar:
					return new FlashTaskBarNotificationAction();
				case ActionType.PlaySound:
					return new PlaySoundNotificationAction();
				case ActionType.ShowToast:
					return new ShowToastNotificationAction();
				default:
					return new FlashTaskBarNotificationAction();
			}
		}

		public ComboViewModel<ActionType>[] ActionTypes { get; private set; }

		public NotificationAction Action
		{
			get {
				return _notificationAction;
			}
		}

		public void Commit()
		{
			if (SubViewModel != null)
			{
				SubViewModel.Commit();
			}
			_notificationAction.ActionCondition = SelectedActionCondition.Data;
			_notificationAction.Interval = Interval;
		}
	}
}