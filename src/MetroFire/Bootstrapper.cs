using System.Linq;
using Castle.Facilities.Startable;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ReactiveUI;
using System;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI.Notifications;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels;
using Rogue.MetroFire.UI.Views;
using Castle.Facilities.TypedFactory;

namespace Rogue.MetroFire.UI
{
	public class Bootstrapper
	{
		private readonly WindsorContainer _container;

		public Bootstrapper()
		{
			_container = new WindsorContainer();
		}

		public bool TestMode { get; set; }

		public WindsorContainer Container { get { return _container; } }

		public IShellWindow Bootstrap()
		{
			_container.Kernel.ComponentModelBuilder.RemoveContributor(
				_container.Kernel.ComponentModelBuilder.Contributors.OfType<PropertiesDependenciesModelInspector>().Single());

			_container.AddFacility<StartableFacility>();
			_container.AddFacility<TypedFactoryFacility>();


			if (!TestMode)
			{
				_container.Register(Component.For<Func<NotificationAction, INotificationAction>>().Instance(Create));
				_container.Install(FromAssembly.This());
			}

			_container.Install(FromAssembly.Containing<RequestLoginMessage>());

			_container.Register(Component.For<IInlineUploadView>().LifestyleTransient().ImplementedBy<InlineUploadView>());
			_container.Register(
				AllTypes.FromThisAssembly().Where(t => t.Namespace == typeof (RoomModuleViewModel).Namespace).
					WithServiceAllInterfaces().LifestyleTransient());

			_container.Register(Component.For<ISettingsLoader>().ImplementedBy<SettingsPersistence>().Forward<ISettingsSaver>().LifestyleTransient());

			_container.Register(Component.For<IChatDocument>().ImplementedBy<ChatDocument>().LifestyleTransient());
			_container.Register(Component.For<IMessageBus>().ImplementedBy<MessageBus>());
			_container.Register(Component.For<IInlineUploadViewFactory>().AsFactory());
			_container.Register(Component.For<INotification>().ImplementedBy<Notification>().LifestyleTransient());
			_container.Register(Component.For<FlashTaskBarAction>().ImplementedBy<FlashTaskBarAction>().LifestyleTransient());
			_container.Register(Component.For<ShowToastAction>().ImplementedBy<ShowToastAction>().LifestyleTransient());
			_container.Register(Component.For<PlaySoundAction>().ImplementedBy<PlaySoundAction>().LifestyleTransient());
			_container.Register(Component.For<ITaskBar>().ImplementedBy<TaskBar>().LifestyleSingleton());
			_container.Register(Component.For<IImageView>().LifestyleTransient().ImplementedBy<ImageView>());
			_container.Register(Component.For<ISettings>().ImplementedBy<CurrentSettings>().Forward<CampfireClient.ISettings>()
				.LifestyleSingleton());
			_container.Register(Component.For<IPasteViewFactory>().ImplementedBy<PasteViewFactory>());
			_container.Register(Component.For<IPasteView>().ImplementedBy<PasteView>().LifestyleTransient());
			_container.Register(Component.For<IApplicationDeployment>().ImplementedBy<ClickOnceApplicationDeployment>());

			_container.Register(AllTypes.FromThisAssembly().Where(t => !t.Namespace.EndsWith("Views")).WithServiceAllInterfaces());
			_container.Register(AllTypes.FromThisAssembly().Where(t => 
				t.Namespace.EndsWith("Views") && !t.Name.In(ModuleNames.RoomModule)).WithServiceAllInterfaces());

			_container.Register(Component.For<IWindsorContainer>().Instance(_container));


			var messageBus = _container.Resolve<IMessageBus>();


			messageBus.Listen<ApplicationLoadedMessage>().Subscribe(
				msg => messageBus.SendMessage(new ActivateMainModuleMessage(ModuleNames.Login)));

			return _container.Resolve<IShellWindow>();
		}

		public class PasteViewFactory : IPasteViewFactory
		{
			private readonly IWindsorContainer _container;

			public PasteViewFactory(IWindsorContainer container)
			{
				_container = container;
			}

			public IPasteView Create(IRoom room, ClipboardItem clipboardItem)
			{
				var vm = _container.Resolve<IPasteViewModel>(new {room, clipboardItem});
				var view = _container.Resolve<IPasteView>(new {vm});

				return view;
			}

			public void Release(IPasteView view)
			{
				var vm = view.Element.DataContext;
				_container.Release(view);
				_container.Release(vm);
			}
		}


		private INotificationAction Create(NotificationAction notificationAction)
		{
			switch (notificationAction.ActionType)
			{
					case ActionType.FlashTaskbar:
					return _container.Resolve<FlashTaskBarAction>(new {data = notificationAction});
					case ActionType.PlaySound:
					return _container.Resolve<PlaySoundAction>(new {data = notificationAction});
					case ActionType.ShowToast:
					return _container.Resolve<ShowToastAction>(new {data = notificationAction});
					default:
					return null;
			}
		}

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}
	}
}
