using System.Windows;

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

	public interface IModule
	{
		string Caption { get; }
		DependencyObject Visual { get; }
	}

	public interface IMainModule : IModule
	{
		
	}

	public interface IModuleResolver
	{
		IModule ResolveModule(string name);
	}
}
