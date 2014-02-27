using System;
using System.Windows.Media;
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
	[XmlInclude(typeof(HighlightTextNotificationAction))]
	public abstract class NotificationAction
	{
		protected NotificationAction()
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
			SecondsVisible = 5;
		}

		public int SecondsVisible { get; set; }
	}

	public class PlaySoundNotificationAction : NotificationAction
	{
		public PlaySoundNotificationAction()
		{
			ActionType = ActionType.PlaySound;
		}

		public string SoundFile { get; set; }
	}

	public class HighlightTextNotificationAction : NotificationAction
	{
		public HighlightTextNotificationAction()
		{
			ActionType = ActionType.HighlightText;
		}

		[XmlIgnore]
		public Color Color { get; set; }

		public int ColorValue
		{
			get
			{
				return BitConverter.ToInt32(new[] {Color.A, Color.R, Color.G, Color.B}, 0);
			}
			set
			{
				var bytes = BitConverter.GetBytes(value);
				Color = Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
			}
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
