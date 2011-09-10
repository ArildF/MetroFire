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
	}

	public interface IRoomModuleViewModel
	{
		string RoomName { get; }
	}

	public interface IRoomModuleCreator
	{
		IModule CreateRoomModule(RoomModuleViewModel room);
	}

	public interface IModule
	{
		string Caption { get; }
		DependencyObject Visual { get; }
	}

	public interface IMainModule : IModule
	{
		
	}

	public interface ICampfireModule : IModule
	{
		
	}

	public interface ILobbyModule : ICampfireModule
	{}

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
}
