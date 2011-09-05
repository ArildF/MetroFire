using System.Windows;

namespace Rogue.MetroFire.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var shellView = new Bootstrapper().Bootstrap();

			shellView.Window.Show();
		}
	}
}
