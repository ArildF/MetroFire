﻿using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Moq;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI.Notifications;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels;
using System;
using System.Linq;
using Rogue.MetroFire.UI.Views;
using Component = Castle.MicroKernel.Registration.Component;

namespace MetroFire.Specs.Steps
{
	public class RoomContext
	{
		private int _currentRoomId;
		private readonly IMessageBus _bus;
		private readonly IList<object> _messages = new List<object>();
		private readonly WindsorContainer _container;
		private readonly List<RoomModuleViewModel> _roomViewModels = new List<RoomModuleViewModel>();
		private readonly CampfireApiFake _campfireApiFake;
		private LobbyModuleViewModel _lobbyModuleViewModel;
		private MainCampfireViewModel _mainCampfireViewModel;
		private readonly MockSettingsLoader _loader;

		private Mock<INotificationAction> _mockFlashTaskBarAction;
		private readonly Dictionary<Type, int> _componentCounts;
		private FakeClipBoardService _fakeClipboardService;

		public RoomContext(CampfireApiFake campfireApiFake)
		{
			var bootstrapper = new Bootstrapper {TestMode = true};
			_container = bootstrapper.Container;
			_container.Kernel.ComponentCreated += KernelOnComponentCreated;

			_campfireApiFake = campfireApiFake;
			_container.Register(Component.For<ICampfireApi>().Instance(_campfireApiFake));

			_loader = new MockSettingsLoader();
			_container.Register(Component.For<ISettingsLoader>().Instance(_loader));

			_container.Register(Component.For<Func<NotificationAction, INotificationAction>>().Instance(CreateNotificationAction));

			_container.Register(
				Component.For<IApplicationActivator>().Instance(
					(ApplicationActivatorMock = new Mock<IApplicationActivator>()).Object));

			_fakeClipboardService = new FakeClipBoardService();
			_container.Register(Component.For<IClipboardService>().Instance(_fakeClipboardService));

			_container.Register(Component.For<IPasteView>().ImplementedBy<FakePasteView>().LifestyleTransient());

			bootstrapper.Bootstrap();



			_container.Install(FromAssembly.This());

			_bus = _container.Resolve<IMessageBus>();


			Listen<RequestRecentMessagesMessage>();

			_bus.SendMessage(new ApplicationLoadedMessage());

			var dummy = _container.Resolve<IToastWindow>();


			_componentCounts = new Dictionary<Type, int>();
			_container.Kernel.ComponentCreated += (model, instance) =>
				{
					int count = 0;
					_componentCounts.TryGetValue(instance.GetType(), out count);
					count++;
					_componentCounts[instance.GetType()] = count;
				};
			_container.Kernel.ComponentDestroyed += (model, instance) =>
				{
					int count = _componentCounts[instance.GetType()];
					count--;
					_componentCounts[instance.GetType()] = count;
				};
		}

		private void CountCreatedComponents(ComponentModel model, object instance)
		{
			throw new NotImplementedException();
		}

		private INotificationAction CreateNotificationAction(NotificationAction arg)
		{
			switch (arg.ActionType)
			{
					case ActionType.FlashTaskbar:
					return (_mockFlashTaskBarAction = new Mock<INotificationAction>()).Object;
					case ActionType.ShowToast: return new ShowToastAction((ShowToastNotificationAction)arg, _bus);
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
			if (instance is LoginViewModel)
			{
				LoginViewModel = (LoginViewModel) instance;
			}
			if (instance is ToastWindowViewModel)
			{
				ToastWindowViewModel = (ToastWindowViewModel) instance;
			}
		}

		public ToastWindowViewModel ToastWindowViewModel { get; set; }

		public LoginViewModel LoginViewModel { get; private set; }

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
			var chatDocument = ((ChatDocument) ViewModelFor(roomName).ChatDocument);
			return chatDocument.Blocks.OfType<Paragraph>()
				.Select(p => p.GetText()).Select(str => new Message {Body = str});
		}

		public RoomModuleViewModel ViewModelFor(string roomName)
		{
			return _roomViewModels.FirstOrDefault(vm => vm.RoomName == roomName);
		}

		public void JoinRoom(string roomName)
		{
			_lobbyModuleViewModel.Rooms.Single(r => r.Name == roomName).JoinCommand.Execute(null);
		}

		public void SendRoomMessage(string username, string message, string roomName)
		{
			var user = _campfireApiFake.Users().First(u => u.Name == username);
			_campfireApiFake.NewRoomMessage(message, roomName, userId: user.Id);
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

		public MetroFireSettings MetroFireSettings
		{
			get { return _loader.Settings; }
		}

		public Mock<IApplicationActivator> ApplicationActivatorMock { get; private set; }

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

		public void PulseSettingsChanged()
		{
			SendMessage(new SettingsChangedMessage());
		}

		public User GetUser(string user)
		{
			return _campfireApiFake.Users().FirstOrDefault(u => u.Name == user);
		}

		public void AddUser(string userName)
		{
			var user = _campfireApiFake.AddUser(userName);
			_bus.SendMessage(new UserInfoReceivedMessage(user));
		}

		public int NumComponentsOfType<T>()
		{
			int num = 0;
			_componentCounts.TryGetValue(typeof (T), out num);
			return num;

		}

		public void PutImageOnClipboard()
		{
			_fakeClipboardService.SetImage();
		}
	}

	public class FakePasteView : FrameworkElement, IPasteView
	{
		private readonly PasteViewModel _vm;
		private readonly IObservable<Unit> _closing;

		public FakePasteView(IPasteViewModel vm)
		{
			_vm = (PasteViewModel) vm;
			_closing = Observable.FromEventPattern<PropertyChangedEventArgs>(vm, "PropertyChanged")
				.Where(_ => _vm.IsFinished).Select(_ => Unit.Default);
			DataContext = vm;
		}

		public FrameworkElement Element { get { return this; } }

		public IObservable<Unit> Closing
		{
			get { return _closing; }
		}
	}

	public class FakeClipBoardService : IClipboardService
	{
		private ClipboardItem _currentItem;

		public ClipboardItem GetClipboardItem()
		{
			return _currentItem;
		}

		public void SetImage()
		{
			_currentItem = new ClipboardItem("Test.png", "image/png");
		}
	}

	public class ModulesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IMainModule>().Named(ModuleNames.Login).LifestyleTransient().UsingFactoryMethod(k =>
				{
					var ignored = k.Resolve<ILoginViewModel>();
					return new Mock<IMainModule>().Object;
				}));
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
