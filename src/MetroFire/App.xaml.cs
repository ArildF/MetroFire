using System.Windows;
using Rogue.MetroFire.UI.Views;

namespace Rogue.MetroFire.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			new ShellView().Show();
		}
	}
}
