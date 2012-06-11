using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.CampfireClient;

namespace Rogue.MetroFire.UI.Notifications
{
	public class Trigger
	{
		private readonly NotificationTrigger _notificationTrigger;

		private readonly Regex _textRegex;

		private readonly Regex _userRegex;

		private readonly Regex _roomRegex;

		public Trigger(NotificationTrigger notificationTrigger)
		{
			_notificationTrigger = notificationTrigger;
			_userRegex = CreateRegex(_notificationTrigger.MatchUser);
			_roomRegex = CreateRegex(_notificationTrigger.MatchRoom);
			_textRegex = CreateRegex(_notificationTrigger.MatchText);
		}

		private Regex CreateRegex(string regexText)
		{
			return new Regex(regexText ?? "", RegexOptions.IgnoreCase);
		}

		public bool Matches(NotificationMessage notificationMessage)
		{
			if (!IsMatchingType(notificationMessage.Message.Type))
			{
				return false;
			}
			var misMatches = from m in new[]
				{
					new {Regex = _roomRegex, Text = notificationMessage.Room.Name},
					new {Regex = _userRegex, Text = notificationMessage.User.Name},
					new {Regex = _textRegex, Text = notificationMessage.Message.Body}
				}
				where !m.Regex.IsMatch(m.Text)
				select m;

			if (misMatches.Any())
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
	}
}