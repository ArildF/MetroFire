using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Rogue.MetroFire.CampfireClient
{
	public class WindsorInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<CampfireApiLoggingInterceptor>().ImplementedBy<CampfireApiLoggingInterceptor>());
			container.Register(Component.For<ICampfireApi>().ImplementedBy<CampfireApi>().Interceptors<CampfireApiLoggingInterceptor>());
			container.Register(AllTypes.FromThisAssembly().Pick().WithServiceAllInterfaces());
		}
	}
}
