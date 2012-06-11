using Castle.Windsor;

namespace Rogue.MetroFire.UI
{
	public class ModuleResolver : IModuleResolver
	{
		private IWindsorContainer _container;

		public ModuleResolver(IWindsorContainer container)
		{
			_container = container;
		}

		public IModule ResolveModule(string name)
		{
			return _container.Resolve<IMainModule>(name);
		}

		public void ReleaseModule(IModule module)
		{
			_container.Release(module);
		}
	}
}
