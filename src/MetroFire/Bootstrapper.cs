using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Rogue.MetroFire.UI
{
	public class Bootstrapper
	{
		private readonly WindsorContainer _container;

		public Bootstrapper()
		{
			_container = new WindsorContainer();
		}

		public IShellWindow Bootstrap()
		{
			_container.Register(AllTypes.FromThisAssembly().Pick().WithServiceAllInterfaces());


			return _container.Resolve<IShellWindow>();
		}
	}
}
