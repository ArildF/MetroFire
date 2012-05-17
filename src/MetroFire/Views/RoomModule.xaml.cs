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

		#region IsConnected

		/// <summary>
		/// IsConnected Dependency Property
		/// </summary>
		public static readonly DependencyProperty IsConnectedProperty =
			DependencyProperty.Register("IsConnected", typeof(bool), typeof(RoomModule),
				new FrameworkPropertyMetadata((bool)false,
					OnIsConnectedChanged));

		/// <summary>
		/// Gets or sets the IsConnected property.  This dependency property 
		/// indicates ....
		/// </summary>
		public bool IsConnected
		{
			get { return (bool)GetValue(IsConnectedProperty); }
			set { SetValue(IsConnectedProperty, value); }
		}

		/// <summary>
		/// Handles changes to the IsConnected property.
		/// </summary>
		private static void OnIsConnectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RoomModule)d).OnIsConnectedChanged(e);
		}

		/// <summary>
		/// Provides derived classes an opportunity to handle changes to the IsConnected property.
		/// </summary>
		protected virtual void OnIsConnectedChanged(DependencyPropertyChangedEventArgs e)
		{
			var newValue = (bool) e.NewValue;
			SetConnectionState(newValue);
		}

		private void SetConnectionState(bool newValue)
		{
			VisualStateManager.GoToState(this, newValue ? "Connected" : "Disconnected", true);
		}

		#endregion

		

			

		public RoomModule()
		{
			InitializeComponent();


			Loaded += OnLoaded;

			
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			SetConnectionState(IsConnected);

			_textBox.Focus();
		}

		public RoomModule(IRoomModuleViewModel vm) : this()
		{
			_vm = vm;
			DataContext = vm;

			var binding = new Binding("IsConnected") {Mode = BindingMode.OneWay, Source = vm};
			SetBinding(IsConnectedProperty, binding);
		}

		public string Caption
		{
			get { return _vm.RoomName; }
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
			get { return _vm.Id; }
		}

		public string Notifications
		{
			get { return _vm.Notifications; }
		}

		public bool Closable
		{
			get { return true; }
		}
	}
}
