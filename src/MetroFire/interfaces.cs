using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Reactive;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.GitHub;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI.Settings;
using Rogue.MetroFire.UI.Twitter;
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


	public interface IModuleCreator
	{
		IModule CreateRoomModule(Room room);
		void ReleaseModule(IModule module);
	}

	public interface IModule
	{
		string Caption { get; }
		bool IsActive { get; set; }
		int Id { get; }
		string Notifications { get; }
		bool Closable { get; }
	}

	public interface IMainModule
	{
		object NavigationContent { get; }
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

	
	public interface ISettingsViewModel
	{
		ICommand SettingsCommand { get; }
	}

	public interface INavigationContentViewModel
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
		IMainModule ResolveMainModule(string name);
		void ReleaseModule(IMainModule module);
	}

	public interface ILoginInfoStorage
	{
		LoginInfo GetStoredLoginInfo();
		void PersistLoginInfo(LoginInfo info);
	}

	public interface IChatDocument 
	{
		object AddMessage(Message message, User user, IRoom room, object textObject);
		void UpdateMessage(object textObject, Message message, User user, IRoom room);
		void AddUploadFile(IRoom room, FileItem fileItem);
		Double FontSize { get; set; }

		void RemoveMessage(object textObject);
		void AddNotificationMessage(string message, bool isError);
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
		bool ShouldTriggerOnSelfMessage { get; }
		bool IsRenderTime { get; }
	}

	public interface INotification
	{
		void Process(NotificationMessage notificationMessage);
		void OnRender(NotificationMessage message, IMessageRenderer renderer);
	}
	public interface IInlineUploadView
	{
		UIElement Element { get; }

	}

	public interface IInlineUploadViewFactory
	{
		IInlineUploadViewModel Create(Message message);
		IDirectLinkInlineUploadViewModel Create(HeadInfo info);
		IInlineUploadView Create(IInlineUploadViewModel vm);
	}

	public interface IDirectLinkInlineUploadViewModel : IInlineUploadViewModel
	{
		
	}


	public interface IWebBrowser
	{
		void NavigateTo(Uri uri);
	}


	public interface IImageView
	{
		void Show();
	}

	public interface IPasteView
	{
		FrameworkElement Element { get; }
		IObservable<Unit> Closing { get; }
	}

	public interface IPasteViewFactory
	{
		IPasteView Create(IRoom room, FileItem fileItem);
		void Release(IPasteView view);
		ICollapsibleTextPasteView CreateTextPasteView(Inline inline);
	}

	public interface ITweetViewFactory
	{
		ITweetView Create(Tweet tweet);
	}

	public interface ITweetView
	{
		FrameworkElement Element { get; }
	}

	public interface ICollapsibleTextPasteView
	{
		FrameworkElement Element { get; }
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
		string GetFilePath();
	}

	public interface IImageEncoder
	{
		string EncodeToTempPng(BitmapSource imageSource);
	}


	public interface IClipboardService
	{
		FileItem GetFileItem();
	}

	public interface IMimeTypeResolver
	{
		string GetMimeType(string path);
	}

	public interface IApplicationDeployment
	{
		bool IsNetworkDeployed { get; }
		void CheckForUpdateAsync();
		void UpdateAsync();
		event CheckForUpdateCompletedEventHandler CheckForUpdateCompleted;
		event AsyncCompletedEventHandler UpdateCompleted;
		event DeploymentProgressChangedEventHandler UpdateProgressChanged;
	}

	public interface IMessageFormatter
	{
		bool ShouldHandle(Message message, User user);
		void Render(Paragraph paragraph, Message message, User user, IRoom room);
		int Priority { get; }
	}

	public interface IMessagePostProcessor
	{
		void Process(Paragraph paragraph, Message message, User user, IRoom room);
		int Priority { get; }
	}

	public interface IGitHubClient
	{
		Commit[] GetLatestCommits();
	}

	public interface IMessageRenderer
	{
		void Highlight(Color color);
	}

	public interface IFormatter
	{
		void Format(NotificationMessage message, IMessageRenderer renderer);
	}

	public interface IRenderAction
	{
		void Format(NotificationMessage message, IMessageRenderer renderer);
	}


	public interface ITwitterClient
	{
		OEmbed GetTweet(string id);
	}
}
