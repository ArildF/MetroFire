using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for LobbyModule.xaml
	/// </summary>
	public partial class LobbyModule : ILobbyModule
	{
		private readonly ILobbyModuleViewModel _vm;

		public LobbyModule()
		{
			InitializeComponent();
		}

		public LobbyModule(ILobbyModuleViewModel vm) : this()
		{
			_vm = vm;
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

		public bool IsActive
		{
			get { return _vm.IsActive; }
			set { _vm.IsActive = value; }
		}

		public int Id
		{
			get { return ModuleIds.Lobby; }
		}

		public string Notifications
		{
			get { return ""; }
		}

		public bool Closable
		{
			get { return false; }
		}
	}

}
