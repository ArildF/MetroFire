using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for RoomModule.xaml
	/// </summary>
	public partial class RoomModuleView 
	{
		private readonly FlowDocument _emptyDocument = new FlowDocument();

		public RoomModuleView()
		{
			InitializeComponent();


			Loaded += OnLoaded;

			PreviewTextInput += OnPreviewTextInput;

			Unloaded += OnUnloaded;

		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			// the tab control will recreate the views every now and then
			// so can't leave this particular view using the ChatDocument from this particular VM
			_chatViewer.Document = _emptyDocument;
		}

		private void OnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
		{
			_textBox.Focus();
		}


		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_textBox.Focus();
			_chatViewer.SetBinding(FlowDocumentScrollViewer.DocumentProperty, "ChatDocument");
		}



	}
}
