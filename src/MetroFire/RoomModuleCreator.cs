using Castle.Windsor;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI
{
	public class RoomModuleCreator : IRoomModuleCreator
	{
		private readonly IWindsorContainer _container;
		private readonly IMessageBus _bus;

		public RoomModuleCreator(IWindsorContainer container, IMessageBus bus)
		{
			_container = container;
			_bus = bus;
		}

		public IModule CreateRoomModule(RoomModuleViewModel vm)
		{
			var module = _container.Resolve<IModule>(ModuleNames.RoomModule, new {vm});

			_bus.SendMessage(new RoomModuleCreatedMessage(module));
			return module;
		}
	}
}
