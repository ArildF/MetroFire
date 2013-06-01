using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LogModuleViewModel : ReactiveObject, ILogModule
	{
		private bool _isActive;

		private readonly ICampfireLog _log;

		public LogModuleViewModel(ICampfireLog log)
		{
			_log = log;
			_log.Updated.SubscribeUI(_ => this.RaisePropertyChanged(vm => vm.Text));
		}

		public string Text
		{
			get { return _log.Text; }
		}

		public string Caption { get { return "Log"; } }

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}

		public int Id { get { return -1; } }
		public string Notifications {
			get { return ""; }
		}
		public bool Closable { get { return false; } }
	}

}
