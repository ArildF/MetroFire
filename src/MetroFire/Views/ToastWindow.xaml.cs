using System;
using System.Windows;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for ToastWindow.xaml
	/// </summary>
	public partial class ToastWindow : IToastWindow
	{
		private ToastWindow()
		{
			InitializeComponent();

			SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			Top = 0;
			Height = SystemParameters.PrimaryScreenHeight;
			Left = SystemParameters.PrimaryScreenWidth - ActualWidth;
		}

		public ToastWindow(IToastWindowViewModel vm) : this()
		{
			DataContext = vm;
		}

		public Window Window
		{
			get { return this; }
		}
	}

}
