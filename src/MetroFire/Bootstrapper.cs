using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient;

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
			_container.AddFacility<StartableFacility>();

			_container.Install(FromAssembly.This());
			_container.Install(FromAssembly.Containing<RequestLoginMessage>());


			_container.Register(Component.For<IMessageBus>().ImplementedBy<MessageBus>());
			_container.Register(AllTypes.FromThisAssembly().Pick().WithServiceAllInterfaces());

			_container.Register(Component.For<IWindsorContainer>().Instance(_container));

			var messageBus = _container.Resolve<IMessageBus>();


			messageBus.Listen<ApplicationLoadedMessage>().Subscribe(
				msg => messageBus.SendMessage(new ActivateMainModuleMessage(ModuleNames.Login)));

			return _container.Resolve<IShellWindow>();
		}
	}
}
