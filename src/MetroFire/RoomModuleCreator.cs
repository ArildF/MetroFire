using Castle.Windsor;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI
{
	public class RoomModuleCreator : IRoomModuleCreator
	{
		private readonly IWindsorContainer _container;

		public RoomModuleCreator(IWindsorContainer container)
		{
			_container = container;
		}

		public IModule CreateRoomModule(RoomModuleViewModel vm)
		{
			var module = _container.Resolve<IModule>(ModuleNames.RoomModule, new {vm});
			return module;
		}
	}
}
