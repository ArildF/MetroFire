using Castle.Windsor;

namespace Rogue.MetroFire.UI
{
	public class ModuleResolver : IModuleResolver
	{
		private readonly IWindsorContainer _container;

		public ModuleResolver(IWindsorContainer container)
		{
			_container = container;
		}

		public IModule ResolveModule(string name)
		{
			return _container.Resolve<IModule>(name);
		}

		public IMainModule ResolveMainModule(string name)
		{
			return _container.Resolve<IMainModule>(name);
		}

		public void ReleaseModule(IMainModule module)
		{
			_container.Release(module);
		}

		public void ReleaseModule(IModule module)
		{
			_container.Release(module);
		}
	}
}
