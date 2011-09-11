using System;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class RoomModuleViewModel : ReactiveObject, IRoomModuleViewModel
	{
		private readonly IRoom _room;
		private readonly IMessageBus _bus;
		private bool _isActive;
		private bool _userEditingMessage;

		private const string DefaultMessage = "Type your message here or paste/drop files\u00A0";
		private string _userMessage;
		private bool _userEditedMessage;

		public RoomModuleViewModel(IRoom room, IMessageBus bus)
		{
			_room = room;
			_bus = bus;

			UserMessage = DefaultMessage;

			PostMessageCommand = new ReactiveCommand(this.ObservableForProperty(vm => vm.UserEditedMessage)
				.Select(c => c.Value).StartWith(false));
			PostMessageCommand.Subscribe(HandlePostMessage);
		}

		private void HandlePostMessage(object o)
		{
			_bus.SendMessage(new RequestSpeakInRoomMessage(Id, UserMessage));
			UserMessage = String.Empty;
		}

		public ReactiveCommand PostMessageCommand { get; set; }

		public string RoomName
		{
			get { return _room.Name; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}

		public string UserMessage
		{
			get { return _userMessage; }
			set
			{
				this.RaiseAndSetIfChanged(vm => vm.UserMessage, ref _userMessage, value);
				UserEditedMessage = value != DefaultMessage && !String.IsNullOrEmpty(value);
			}
		}

		public bool UserEditingMessage
		{
			get { return _userEditingMessage; }
			set
			{
				if (value && UserMessage == DefaultMessage)
				{
					UserMessage = string.Empty;
				}
				if (!value && String.IsNullOrEmpty(UserMessage))
				{
					UserMessage = DefaultMessage;
				}
				this.RaiseAndSetIfChanged(vm => vm.UserEditingMessage, ref _userEditingMessage, value);
			}
		}

		public bool UserEditedMessage
		{
			get { return _userEditedMessage; }
			set { this.RaiseAndSetIfChanged(vm => vm.UserEditedMessage, ref _userEditedMessage, value); }
		}

		public int Id
		{
			get { return _room.Id; }
		}
	}

}
