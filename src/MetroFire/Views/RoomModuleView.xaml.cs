using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for RoomModule.xaml
	/// </summary>
	public partial class RoomModuleView 
	{

		public RoomModuleView()
		{
			InitializeComponent();


			Loaded += OnLoaded;

			PreviewTextInput += OnPreviewTextInput;
		}

		private void OnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
		{
			_textBox.Focus();
		}


		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_textBox.Focus();
		}


	}
}
