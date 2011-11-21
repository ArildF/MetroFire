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
	/// Interaction logic for LogModule.xaml
	/// </summary>
	public partial class LogModule : ILogModule
	{
		private readonly ILogViewModel _vm;

		public LogModule()
		{
			InitializeComponent();
		}

		public LogModule(ILogViewModel vm) : this()
		{
			_vm = vm;
			DataContext = vm;
		}

		public string Caption
		{
			get { return "Log"; }
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
			get { return -2; }
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
