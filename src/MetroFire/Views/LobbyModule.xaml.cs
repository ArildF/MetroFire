using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for LobbyModule.xaml
	/// </summary>
	public partial class LobbyModule : ILobbyModule
	{
		public LobbyModule()
		{
			InitializeComponent();
		}

		public LobbyModule(ILobbyModuleViewModel vm) : this()
		{
			DataContext = vm;
		}

		public string Caption
		{
			get { return "Lobby"; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}
	}

}
