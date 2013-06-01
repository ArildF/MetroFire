using System.Collections.Generic;
using System.Web.Configuration;
using Castle.Windsor;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI
{
	public class ModuleCreator : IModuleCreator
	{
		private readonly IWindsorContainer _container;
		private readonly IMessageBus _bus;
		private readonly IDictionary<IModule, object> _roomModules = new Dictionary<IModule, object>();

		public ModuleCreator(IWindsorContainer container, IMessageBus bus)
		{
			_container = container;
			_bus = bus;
		}

		public IModule CreateRoomModule(Room room)
		{
			var module = _container.Resolve<IModule>(ModuleNames.RoomModule, new {room});

			_bus.SendMessage(new RoomModuleCreatedMessage(module));
			return module;
		}

		public void ReleaseModule(IModule module)
		{
			_container.Release(module);

			_roomModules.Remove(module);
		}
	}
}
