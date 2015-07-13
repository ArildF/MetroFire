using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.ViewModels;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for InlineTweetView.xaml
	/// </summary>
	public partial class InlineTweetView : ITweetView
	{
		private InlineTweetView()
		{
			InitializeComponent();
		}

		public InlineTweetView(InlineTweetViewModel vm) : this()
		{
			DataContext = vm;
		}


		public FrameworkElement Element
		{
			get { return this; }
		}
	}
}