using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Notifications
{
	public class Trigger
	{
		private readonly NotificationTrigger _notificationTrigger;

		private static readonly Dictionary<TriggerType, MessageType[]> TriggerMatches =
			new Dictionary<TriggerType, MessageType[]>
				{
					{TriggerType.UserEnters, new[] {MessageType.EnterMessage}},
					{TriggerType.UserLeaves, new[] {MessageType.LeaveMessage, MessageType.KickMessage}},
					{
						TriggerType.UserEntersOrLeaves,
						new[] {MessageType.EnterMessage, MessageType.LeaveMessage, MessageType.KickMessage}
						},
					{
						TriggerType.RoomActivity, new[]
							{
								MessageType.EnterMessage, MessageType.LeaveMessage, MessageType.KickMessage,
								MessageType.PasteMessage, MessageType.UploadMessage, MessageType.TextMessage
							}
						},
					{TriggerType.UserMessage, new[] {MessageType.PasteMessage, MessageType.TextMessage, MessageType.UploadMessage}},
					{TriggerType.UserPaste, new[] {MessageType.PasteMessage, MessageType.UploadMessage}}
				};

		public Trigger(NotificationTrigger notificationTrigger)
		{
			_notificationTrigger = notificationTrigger;


		}

		public bool Matches(NotificationMessage notificationMessage)
		{
			if (!IsMatchingType(notificationMessage.Message.Type))
			{
				return false;
			}
			if (_notificationTrigger.MatchRoom != null && 
				!notificationMessage.Room.Name.ToLower().Contains(_notificationTrigger.MatchRoom.ToLower()))
			{
				return false;
			}

			if (_notificationTrigger.MatchUser != null &&
				!notificationMessage.User.Name.ToLower().Contains(_notificationTrigger.MatchUser.ToLower()))
			{
				return false;
			}

			if (_notificationTrigger.MatchText != null &&
				!notificationMessage.Message.Body.ToLower().Contains(_notificationTrigger.MatchText.ToLower()))
			{
				return false;
			}

			return true;
		}

		private bool IsMatchingType(MessageType type)
		{
			MessageType[] types;
			if (!TriggerMatches.TryGetValue(_notificationTrigger.TriggerType, out types))
			{
				return false;
			}
			return type.In(types);
		}
	}
}