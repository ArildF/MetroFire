using System.Xml.Serialization;

namespace Rogue.MetroFire.UI.Settings
{
	public class NotificationSettings
	{
		public NotificationEntry[] Notifications { get; set; }
	}

	public class NotificationEntry
	{
		public NotificationTrigger[] Triggers { get; set; }

		public NotificationAction[] Actions { get; set; }
	}

	[XmlInclude(typeof(FlashTaskBarNotificationAction))]
	[XmlInclude(typeof(PlaySoundNotificationAction))]
	[XmlInclude(typeof(ShowToastNotificationAction))]
	public class NotificationAction
	{
		public NotificationAction()
		{
			Interval = 60;
		}
		public ActionType ActionType { get; set; }

		public ActionCondition ActionCondition { get; set; }

		public int Interval { get; set; }
	}

	public class FlashTaskBarNotificationAction : NotificationAction
	{
		public FlashTaskBarNotificationAction()
		{
			ActionType = ActionType.FlashTaskbar;
		}
	}

	public class ShowToastNotificationAction : NotificationAction
	{
		public ShowToastNotificationAction()
		{
			ActionType = ActionType.ShowToast;
		}
	}

	public class PlaySoundNotificationAction : NotificationAction
	{
		public PlaySoundNotificationAction()
		{
			ActionType = ActionType.PlaySound;
		}

		public string SoundFile { get; set; }
	}

	public class NotificationTrigger
	{
		public TriggerType TriggerType { get; set; }
		public string MatchText { get; set; }
		public string MatchRoom { get; set; }
		public string MatchUser { get; set; }
	}
}
