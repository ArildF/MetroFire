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

	public interface IChatDocument
	{
		void AddMessage(string type, string body);
	}

	public interface IRoomModuleViewModelFactory
	{
		IRoomModuleViewModel Create(Room room);
	}
}
