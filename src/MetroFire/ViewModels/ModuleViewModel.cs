using System;
using ReactiveUI;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class ModuleViewModel : ReactiveObject
	{
		private readonly IModule _module;
		private readonly IMessageBus _bus;

		public ModuleViewModel(IModule module, IMessageBus bus)
		{
			_module = module;
			_bus = bus;

			_bus.Listen<RoomActivityMessage>().Delay(TimeSpan.FromMilliseconds(100))
				.SubscribeUI(msg => this.RaisePropertyChanged(vm => vm.Notifications));
		}

		public IModule Module { get { return _module; } }

		public bool IsActive { get { return _module.IsActive; } }
		public string Caption { get { return _module.Caption; } }
		public string Notifications { get { return _module.Notifications; } }
	}
}