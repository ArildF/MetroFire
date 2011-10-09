using ReactiveUI;
using Rogue.MetroFire.UI.Settings;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class PlaySoundViewModel : ReactiveObject, IActionSubViewModel
	{
		private readonly PlaySoundNotificationAction _notificationAction;
		private string _soundFile;

		public PlaySoundViewModel(PlaySoundNotificationAction notificationAction)
		{
			_notificationAction = notificationAction;
		}

		public string SoundFile
		{
			get { return _soundFile; }
			set { this.RaiseAndSetIfChanged(vm => vm.SoundFile, ref _soundFile, value); }
		}


		public void Commit()
		{
			_notificationAction.SoundFile = SoundFile;
		}
	}
}