using ReactiveUI;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class LogViewModel : ReactiveObject, ILogViewModel
	{
		private bool _isActive;

		private readonly ICampfireLog _log;

		public LogViewModel(ICampfireLog log)
		{
			_log = log;
			_log.Updated.SubscribeUI(_ => this.RaisePropertyChanged(vm => vm.Text));
		}

		public string Text
		{
			get { return _log.Text; }
		}

		public bool IsActive
		{
			get { return _isActive; }
			set { this.RaiseAndSetIfChanged(vm => vm.IsActive, ref _isActive, value); }
		}


	}

}
