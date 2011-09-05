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
}
