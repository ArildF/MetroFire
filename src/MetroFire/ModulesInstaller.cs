using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rogue.MetroFire.UI.Views;

namespace Rogue.MetroFire.UI
{
	public class ModulesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IMainModule>().Named(ModuleNames.Login).LifestyleTransient().ImplementedBy<LoginView>());
		}
	}
}
