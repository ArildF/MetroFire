using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for RoomModule.xaml
	/// </summary>
	public partial class RoomModule : IModule
	{
		private readonly IRoomModuleViewModel _vm;

		public RoomModule()
		{
			InitializeComponent();
		}

		public RoomModule(IRoomModuleViewModel vm)
		{
			_vm = vm;
			DataContext = vm;
		}

		public string Caption
		{
			get { return _vm.RoomName; }
		}

		public DependencyObject Visual
		{
			get { return this; }
		}
	}
}
