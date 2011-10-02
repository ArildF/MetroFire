using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
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

	public interface IMainCampfireViewModel
	{
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

	public interface IRoomModuleCreator
	{
		IModule CreateRoomModule(IRoomModuleViewModel room);
	}

	public interface IModule
	{
		string Caption { get; }
		DependencyObject Visual { get; }
		bool IsActive { get; set; }
		int Id { get; }
		string Notifications { get; }
	}

	public interface IMainModule : IModule
	{
		
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
		object AddMessage(Message message, User user);
		void UpdateMessage(object textObject, Message message, User user);
	}

	public interface IUserCache
	{
		User GetUser(int id, User existingUser);
		bool UserUpdated(User oldUser, User newUser);
	}

	public interface IRoomModuleViewModelFactory
	{
		IRoomModuleViewModel Create(Room room);
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
}
