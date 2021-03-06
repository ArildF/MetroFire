﻿using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Castle.Facilities.Startable;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
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
			_container.Register(Component.For<IMessageBus>().ImplementedBy<MessageBus>());
			_container.Register(Component.For<IMimeTypeResolver>().ImplementedBy<MimeTypeResolver>());
			_container.Register(Component.For<IEmojiProvider>().ImplementedBy<EmojiLoader>());
			
			_container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));

			if (!TestMode)
			{
				_container.AddFacility(new ViewModelRegistrationFacility(Application.Current));
				_container.Register(Component.For<Func<NotificationAction, INotificationAction>>().Instance(Create));

				_container.Register(Component.For<ResourceDictionary>().Instance(Application.Current.Resources));
			}
			_container.Install(FromAssembly.This());


			_container.Register(AllTypes.FromThisAssembly().BasedOn<IMessageFormatter>().WithService.AllInterfaces());

			_container.Install(FromAssembly.Containing<RequestLoginMessage>());

			_container.Register(Component.For<IInlineUploadView>().LifestyleTransient().ImplementedBy<InlineUploadView>());
			_container.Register(
				Component.For<INavigationContentViewModel>().ImplementedBy<NavigationContentViewModel>().LifeStyle.Singleton);
			_container.Register(
				AllTypes.FromThisAssembly().Where(t => t.Namespace == typeof (RoomModuleViewModel).Namespace).
					WithServiceAllInterfaces().WithServiceSelf().LifestyleTransient());

			_container.Register(Component.For<ISettingsLoader>().ImplementedBy<SettingsPersistence>().Forward<ISettingsSaver>().LifestyleTransient());

			_container.Register(Component.For<IChatDocument>().ImplementedBy<ChatDocument>().LifestyleTransient());
			_container.Register(Component.For<IInlineUploadViewFactory>().AsFactory());
			_container.Register(Component.For<INotification>().ImplementedBy<Notification>().LifestyleTransient());
			_container.Register(Component.For<FlashTaskBarAction>().ImplementedBy<FlashTaskBarAction>().LifestyleTransient());
			_container.Register(Component.For<ShowToastAction>().ImplementedBy<ShowToastAction>().LifestyleTransient());
			_container.Register(Component.For<PlaySoundAction>().ImplementedBy<PlaySoundAction>().LifestyleTransient());
			_container.Register(Component.For<HighlightTextAction>().ImplementedBy<HighlightTextAction>().LifestyleTransient());
			_container.Register(Component.For<ITaskBar>().ImplementedBy<TaskBar>().LifestyleSingleton());
			_container.Register(Component.For<IImageView>().LifestyleTransient().ImplementedBy<ImageView>());
			_container.Register(Component.For<ISettings>().ImplementedBy<CurrentSettings>().Forward<CampfireClient.ISettings>()
				.LifestyleSingleton());
			_container.Register(Component.For<IPasteViewFactory>().ImplementedBy<PasteViewFactory>());
			_container.Register(Component.For<IPasteView>().ImplementedBy<PasteView>().LifestyleTransient());
			_container.Register(Component.For<ICollapsibleTextPasteView>().ImplementedBy<CollapsibleTextPasteView>().LifestyleTransient());
			_container.Register(Component.For<IApplicationDeployment>().ImplementedBy<ClickOnceApplicationDeployment>());
			//_container.Register(Component.For<IApplicationDeployment>().ImplementedBy<FakeApplicationDeployment>());

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

			public IPasteView Create(IRoom room, FileItem fileItem)
			{
				var vm = _container.Resolve<IPasteViewModel>(new {room, fileItem});
				var view = _container.Resolve<IPasteView>(new {vm});

				return view;
			}

			public void Release(IPasteView view)
			{
				var vm = view.Element.DataContext;
				_container.Release(view);
				_container.Release(vm);
			}

			public ICollapsibleTextPasteView CreateTextPasteView(Inline inline)
			{
				var view = _container.Resolve<ICollapsibleTextPasteView>(new {inline});

				return view;
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
					case ActionType.HighlightText:
					return _container.Resolve<HighlightTextAction>(new {data = notificationAction});
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
