using System.Collections.Generic;
using System.Windows;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels;
using System;
using System.Linq;

namespace MetroFire.Specs.Steps
{
	public class RoomContext
	{
		private int _currentRoomId;
		private readonly IMessageBus _bus;
		private readonly ChatViewFake _chatViewFake ;
		private readonly IList<object> _messages = new List<object>();
		private readonly WindsorContainer _container;
		private readonly List<RoomModuleViewModel> _roomViewModels = new List<RoomModuleViewModel>();
		private readonly CampfireApiFake _campfireApiFake;
		private LobbyModuleViewModel _lobbyModuleViewModel;
		private MainCampfireViewModel _mainCampfireViewModel;
		private readonly MockSettingsLoader _loader;

		private Mock<INotificationAction> _mockFlashTaskBarAction;

		public RoomContext()
		{
			var bootstrapper = new Bootstrapper {TestMode = true};
			_container = bootstrapper.Container;
			_container.Kernel.ComponentCreated += KernelOnComponentCreated;

			_campfireApiFake = new CampfireApiFake();
			_container.Register(Component.For<ICampfireApi>().Instance(_campfireApiFake));

			_chatViewFake = new ChatViewFake();
			_container.Register(Component.For<IChatDocument>().Instance(_chatViewFake));


			_loader = new MockSettingsLoader();
			_container.Register(Component.For<ISettingsLoader>().Instance(_loader));

			_container.Register(Component.For<Func<NotificationAction, INotificationAction>>().Instance(CreateNotificationAction));

			bootstrapper.Bootstrap();


			_container.Install(FromAssembly.This());



			_bus = _container.Resolve<IMessageBus>();


			Listen<RequestRecentMessagesMessage>();

			_bus.SendMessage(new ApplicationLoadedMessage());
		}

		private INotificationAction CreateNotificationAction(NotificationAction arg)
		{
			switch (arg.ActionType)
			{
					case ActionType.FlashTaskbar:
					return (_mockFlashTaskBarAction = new Mock<INotificationAction>()).Object;
			}

			throw new InvalidOperationException();
		}

		private void KernelOnComponentCreated(ComponentModel model, object instance)
		{
			var roomModule = model.Services.FirstOrDefault(t => t == typeof (IRoomModuleViewModel));
			if (roomModule != null)
			{
				_roomViewModels.Add((RoomModuleViewModel) instance);
			}

			if (instance is LobbyModuleViewModel)
			{
				_lobbyModuleViewModel = (LobbyModuleViewModel) instance;
			}
			if (instance is MainCampfireViewModel)
			{
				_mainCampfireViewModel = (MainCampfireViewModel) instance;
			}
		}

		private void Listen<T>()
		{
			_bus.Listen<T>().SubscribeUI(msg => _messages.Add(msg));
		}

		public IEnumerable<object> AllMessages
		{
			get { return _messages; }
		}

		public CampfireApiFake ApiFake
		{
			get {
				return _campfireApiFake;
			}
		}

		public MainCampfireViewModel MainViewModel
		{
			get {
				return _mainCampfireViewModel;
			}
		}

		public Mock<INotificationAction> FlashTaskBarActionMock
		{
			get { return _mockFlashTaskBarAction; }
		}

		public void CreateRoom(string roomName)
		{
			var room = new Room {Id = _currentRoomId++, Name = roomName, Users = new[]{new User{Name = "User0", Id=0}}};

			_campfireApiFake.AddRoom(room);
			_bus.SendMessage(new RequestRoomListMessage());
		}

		public void SendMessage<T>(T message)
		{
			_bus.SendMessage(message);
		}

		public int IdForRoom(string roomName)
		{
			return _campfireApiFake.IdForRoom(roomName);
		}

		public IEnumerable<Message> MessagesDisplayedInRoom(string roomName)
		{
			return _chatViewFake.Messages;
		}

		public RoomModuleViewModel ViewModelFor(string roomName)
		{
			return _roomViewModels.FirstOrDefault(vm => vm.RoomName == roomName);
		}

		public void JoinRoom(string roomName)
		{
			_lobbyModuleViewModel.Rooms.Single(r => r.Name == roomName).JoinCommand.Execute(null);
		}

		public void SendRoomMessage(string message, string roomName)
		{
			_campfireApiFake.NewRoomMessage(message, roomName);
		}

		public void SendRoomMessages(string roomName, params Message[] messages)
		{
			_campfireApiFake.SendRoomMessages(roomName, messages);
		}

		public void ActivateModule(string roomName)
		{
			var vm = _mainCampfireViewModel.CurrentModules.Single(m => m.Module.Caption == roomName);
			_mainCampfireViewModel.ActivateModuleCommand.Execute(vm);
		}

		public void ChangeSettings(MetroFireSettings settings)
		{
			_loader.Settings = settings;
			SendMessage(new SettingsChangedMessage());
		}

		public class MockSettingsLoader : ISettingsLoader
		{
			public MetroFireSettings Settings = new MetroFireSettings
				{
					General = new GeneralSettings(),
					Network = new NetworkSettings(),
					Notification = new NotificationSettings {Notifications = new NotificationEntry[] {}}
				};

			public MetroFireSettings Load()
			{
				return Settings;
			}
		}
	}

	public class ModulesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IMainModule>().Named(ModuleNames.Login).LifestyleTransient().UsingFactoryMethod(_ => new Mock<IMainModule>().Object));
			container.Register(Component.For<IMainModule>().Named(ModuleNames.SettingsModule).LifestyleTransient().UsingFactoryMethod(_ => new Mock<IMainModule>().Object));
			container.Register(Component.For<IMainModule>().Named(ModuleNames.MainCampfireView).LifestyleTransient().UsingFactoryMethod(k =>
				{
					var ignored = k.Resolve<IMainCampfireViewModel>();
					return new Mock<IMainModule>().Object;
				}));
			container.Register(
				Component.For<IModule>().Named(ModuleNames.RoomModule).LifestyleTransient().ImplementedBy<FakeRoomModule>());
		}

		private class FakeRoomModule : IModule
		{
			private IRoomModuleViewModel _vm;

			public FakeRoomModule(IRoomModuleViewModel vm)
			{
				_vm = vm;
			}

			public string Caption
			{
				get { return _vm.RoomName; }
			}

			public DependencyObject Visual
			{
				get { return null; }
			}

			public bool IsActive
			{
				get { return _vm.IsActive; }
				set { _vm.IsActive = value; }
			}

			public int Id
			{
				get { return _vm.Id; }
			}

			public string Notifications
			{
				get { throw new NotImplementedException(); }
			}

			public bool Closable
			{
				get { return true; }
			}
		}
	}
}
