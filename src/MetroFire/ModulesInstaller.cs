using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rogue.MetroFire.UI.ViewModels;
using Rogue.MetroFire.UI.Views;

namespace Rogue.MetroFire.UI
{
	public class ModulesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IMainModule>().Named(ModuleNames.Login).LifestyleTransient().ImplementedBy<LoginViewModel>());
			container.Register(Component.For<IMainModule>().Named(ModuleNames.SettingsModule).LifestyleTransient().ImplementedBy<SettingsModuleViewModel>());
			container.Register(Component.For<IMainModule>().Named(ModuleNames.MainCampfireView).LifestyleTransient().ImplementedBy<MainCampfireViewModel>());
			container.Register(
				Component.For<IModule>().Named(ModuleNames.RoomModule).LifestyleTransient().ImplementedBy<RoomModuleViewModel>());
		}
	}
}
