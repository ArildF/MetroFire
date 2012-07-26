using System;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class TriggerViewModel : ReactiveObject, IToggleEdit
	{
		private readonly NotificationTrigger _trigger;
		private ComboViewModel<TriggerType> _selectedTriggerType;
		private bool _doMatchText;
		private string _matchText;
		private bool _doMatchRoom;
		private string _matchRoom;
		private bool _doMatchUser;
		private string _matchUser;
		private bool _isEditing;

		public TriggerViewModel(NotificationTrigger trigger)
		{
			_trigger = trigger;
			TriggerTypes = new[]
				{
					new ComboViewModel<TriggerType>("there is any activity", TriggerType.RoomActivity), 
					new ComboViewModel<TriggerType>("a user enters", TriggerType.UserEnters), 
					new ComboViewModel<TriggerType>("a user leaves", TriggerType.UserLeaves),
					new ComboViewModel<TriggerType>("a user enters or leaves", TriggerType.UserEntersOrLeaves), 
					new ComboViewModel<TriggerType>("a user posts a message", TriggerType.UserMessage), 
					new ComboViewModel<TriggerType>("a user posts a file or picture", TriggerType.UserPaste), 
				};

			SelectedTriggerType = TriggerTypes.FirstOrDefault(t => t.Data == trigger.TriggerType);
			MatchText = trigger.MatchText;
			DoMatchText = !String.IsNullOrEmpty(MatchText);
			MatchRoom = trigger.MatchRoom;
			DoMatchRoom = !String.IsNullOrEmpty(MatchRoom);
			MatchUser = trigger.MatchUser;
			DoMatchUser = !String.IsNullOrEmpty(MatchUser);


			
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

		public string DisplayText
		{
			get{ return FormatDisplayText(); }
		}

		private string FormatDisplayText()
		{
			return SelectedTriggerType.Text + 
				(DoMatchText ? " matching the text '" + MatchText + "'" : String.Empty);
		}

		public ComboViewModel<TriggerType> SelectedTriggerType
		{
			get { return _selectedTriggerType; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedTriggerType, ref _selectedTriggerType, value); }
		}

		public bool DoMatchText
		{
			get { return _doMatchText; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.DoMatchText, ref _doMatchText, value);
				if (!_doMatchText)
				{
					MatchText = "";
				}
			}
		}

		public string MatchText
		{
			get { return _matchText; }
			set { this.RaiseAndSetIfChanged(vm => vm.MatchText, ref _matchText, value); }
		}

		public bool DoMatchRoom
		{
			get { return _doMatchRoom; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.DoMatchRoom, ref _doMatchRoom, value);
				if (!_doMatchRoom)
				{
					MatchRoom = "";
				}
			}
		}

		public string MatchRoom
		{
			get { return _matchRoom; }
			set { this.RaiseAndSetIfChanged(vm => vm.MatchRoom, ref _matchRoom, value); }
		}

		public bool DoMatchUser
		{
			get { return _doMatchUser; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.DoMatchUser, ref _doMatchUser, value);
				if (!_doMatchUser)
				{
					MatchUser = "";
				}
			}
		}

		public string MatchUser
		{
			get { return _matchUser; }
			set { this.RaiseAndSetIfChanged(vm => vm.MatchUser, ref _matchUser, value); }
		}


		public ComboViewModel<TriggerType>[] TriggerTypes { get; private set; }

		public NotificationTrigger Trigger
		{
			get {
				return _trigger;
			}
		}


		public void Commit()
		{
			_trigger.TriggerType = SelectedTriggerType.Data;
			_trigger.MatchRoom = MatchRoom;
			_trigger.MatchText = MatchText;
			_trigger.MatchUser = MatchUser;
		}
	}
}