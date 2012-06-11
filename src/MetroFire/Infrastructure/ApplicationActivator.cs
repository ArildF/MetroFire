using System.Windows;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class ApplicationActivator : IApplicationActivator
	{
		public void Activate()
		{
			Application.Current.MainWindow.Activate();
		}
	}
}
