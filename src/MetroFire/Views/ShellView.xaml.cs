using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml
	/// </summary>
	public partial class ShellView : IShellWindow
	{
		public ShellView()
		{
			InitializeComponent();
		}

		public ShellView(IShellViewModel vm) : this()
		{
			DataContext = vm;
		}

		public Window Window
		{
			get { return this; }
		}
	}
}
