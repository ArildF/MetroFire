using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI
{
	public interface IWindow
	{
		Window Window { get; }
	}
	
	public interface IShellWindow : IWindow
	{}

	public interface IShellViewModel 
	{}

	public interface ILoginViewModel
	{
	}

	public interface IToastWindow
	{
		Window Window { get; }
	}

	public interface IToastWindowViewModel
	{
	}

	public interface IGlobalCommands
	{
		ReactiveCommand NextModuleCommand { get; }
		ReactiveCommand PreviousModuleCommand { get; }
		ReactiveCommand LeaveRoomCommand { get; }
	}

	public interface IMainCampfireViewModel
	{
		Direction ModuleDirectionRelativeTo(IModule current, IModule module);
	}

	public interface ILobbyModuleViewModel
	{
		bool IsActive { get; set; }
	}

	public interface IRoomModuleViewModel
	{
		string RoomName { get; }
		bool IsActive { get; set; }
		int Id { get; }
		string Notifications { get; }
	}

	public interface IModuleCreator
	{
		IModule CreateRoomModule(Room room);
		void ReleaseModule(IModule module);
	}

	public interface IModule
	{
		string Caption { get; }
		DependencyObject Visual { get; }
		bool IsActive { get; set; }
		int Id { get; }
		string Notifications { get; }
		bool Closable { get; }
	}

	public interface INavigationContent
	{
		DependencyObject Visual { get; }
	}

	public interface IMainModule : IModule
	{
		DependencyObject NavigationContent { get; }
	}

	public interface ICampfireModule : IModule
	{
		
	}

	public interface ILobbyModule : ICampfireModule
	{}

	public interface ILogModule : ICampfireModule
	{
	}

	public interface IInlineUploadViewModel
	{
	}

	public interface ILogViewModel
	{
		bool IsActive { get; set; }
	}

	
	public interface ISettingsViewModel
	{
		
	}

	public interface ICampfireLog
	{
		string Text { get; }
		IObservable<Unit> Updated { get; }
	}

	public interface IModuleResolver
	{
		IModule ResolveModule(string name);
		void ReleaseModule(IModule module);
	}

	public interface ILoginInfoStorage
	{
		LoginInfo GetStoredLoginInfo();
		void PersistLoginInfo(LoginInfo info);
	}

	public interface IChatDocument
	{
		object AddMessage(Message message, User user, object textObject);
		void UpdateMessage(object textObject, Message message, User user);
		void AddPasteFile(IRoom room, string path);
		Double FontSize { get; set; }

	}

	public interface IUserCache
	{
		User GetUser(int id, User existingUser);
		bool UserUpdated(User oldUser, User newUser);
		User GetUser(int userId);
	}

	public interface INotificationAction
	{
		void Execute(NotificationMessage notificationMessage);
	}

	public interface INotification
	{
		void Process(NotificationMessage notificationMessage);
	}
	public interface IInlineUploadView
	{
		UIElement Element { get; }

	}

	public interface IInlineUploadViewFactory
	{
		IInlineUploadViewModel Create(Message message);
		IInlineUploadView Create(IInlineUploadViewModel vm);
	}

	public interface IWebBrowser
	{
		void NavigateTo(Uri uri);
	}

	public interface IImageView
	{
		bool? ShowDialog();
	}

	public interface IPasteView
	{
		FrameworkElement Element { get; }
		IObservable<Unit> Closing { get; }
	}

	public interface IPasteViewFactory
	{
		IPasteView Create(IRoom room, string bitmapSource);
		void Release(IPasteView view);
	}

	public interface IPasteViewModel
	{
	}

	public interface ISettings
	{
		INetworkSettings Network { get; }
		GeneralSettings General { get; }
		NotificationSettings Notification { get; }
	}

	public interface ITaskBar
	{
		void Flash();
	}

	public interface IApplicationActivator
	{
		void Activate();
	}


	public interface IClipboard
	{
		string GetText();
		BitmapSource GetImage();
	}

	public interface IImageEncoder
	{
		string EncodeToTempPng(BitmapSource imageSource);
	}
}
