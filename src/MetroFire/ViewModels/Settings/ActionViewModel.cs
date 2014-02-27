using System.Linq;
using System.Windows.Media;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public interface IActionSubViewModel
	{
		void Commit();
		Color Color { get; }
	}
	public class ActionViewModel : ReactiveObject, IToggleEdit
	{
		private NotificationAction _notificationAction;
		private ComboViewModel<NotificationAction> _selectedAction;
		private ComboViewModel<ActionCondition> _selectedActionCondition;
		private int _interval;
		private bool _showInterval;
		private string _intervalText;
		private IActionSubViewModel _subViewModel;
		private bool _isEditing;

		public ActionViewModel(NotificationAction notificationAction)
		{
			_notificationAction = notificationAction;

			ActionTypes = new[]
				{
					new ComboViewModel<NotificationAction>(Description(ActionType.FlashTaskbar), 
						new FlashTaskBarNotificationAction {ActionType = ActionType.FlashTaskbar}),
					new ComboViewModel<NotificationAction>(Description(ActionType.ShowToast), 
						new ShowToastNotificationAction{ActionType = ActionType.ShowToast}),
					new ComboViewModel<NotificationAction>(Description(ActionType.HighlightText), 
						new HighlightTextNotificationAction{ActionType = ActionType.HighlightText}), 
					//new ComboViewModel<ActionType>("play a sound", ActionType.PlaySound),
				};

			ActionConditions = new[]
				{
					new ComboViewModel<ActionCondition>("always", ActionCondition.None),
					new ComboViewModel<ActionCondition>("when idle for at least", ActionCondition.RoomIdle),
					new ComboViewModel<ActionCondition>("last notification was at least",
					                                    ActionCondition.MinimumTimeSinceLastNotification),
				};

			SelectedAction = new ComboViewModel<NotificationAction>(
				Description(_notificationAction.ActionType), _notificationAction);
			ActionTypes = ActionTypes.Select(
				at => at.Data.ActionType == SelectedAction.Data.ActionType ? SelectedAction : at)
				.ToArray();
			SelectedActionCondition = ActionConditions.FirstOrDefault(ac => ac.Data == notificationAction.ActionCondition);
			Interval = notificationAction.Interval;

		}

		private static string Description(ActionType flashTaskbar)
		{
			switch (flashTaskbar)
			{
				case ActionType.FlashTaskbar:
					return "flash the task bar";
				case ActionType.HighlightText:
					return "highlight text";
				case ActionType.ShowToast:
					return "show a toast";
				default:
					return "";
			}
		}


		public bool IsEditing
		{
			get { return _isEditing; }
			set 
			{
				if (_isEditing == value)
				{
					return;
				}
				this.RaiseAndSetIfChanged(vm => vm.IsEditing, ref _isEditing, value);
				raisePropertyChanged(null);
			}
		}

		public Color DisplayColor
		{
			get { return SubViewModel != null ? SubViewModel.Color : Colors.Black; }
		}


		public string DisplayText
		{
			get { return FormatDisplayText(); }
		}

		private string FormatDisplayText()
		{
			return SelectedAction.Text;
		}

		public ComboViewModel<ActionCondition>[] ActionConditions { get; private set; }


		public ComboViewModel<NotificationAction> SelectedAction
		{
			get { return _selectedAction; }
			set
			{
				if (_selectedAction == value)
				{
					return;
				}
				this.RaiseAndSetIfChanged(vm => vm.SelectedAction, ref _selectedAction, value);

				_notificationAction = value.Data;
				SubViewModel = CreateSubViewModel(value.Data);
				raisePropertyChanged(null);
			}
		}

		private IActionSubViewModel CreateSubViewModel(NotificationAction data)
		{
			switch (data.ActionType)
			{
				case ActionType.PlaySound:
					return new PlaySoundViewModel(
						data as PlaySoundNotificationAction ?? new PlaySoundNotificationAction());
				case ActionType.HighlightText:
					return new HighlightTextViewModel(
						data as HighlightTextNotificationAction ?? new HighlightTextNotificationAction());
				default:
					return null;
			}
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
				case ActionType.HighlightText:
					return new HighlightTextNotificationAction();
				default:
					return new FlashTaskBarNotificationAction();
			}
		}

		public ComboViewModel<NotificationAction>[] ActionTypes { get; private set; }

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