using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for InlineUploadView.xaml
	/// </summary>
	public partial class InlineUploadView : IInlineUploadView
	{
		public InlineUploadView()
		{
			InitializeComponent();
		}

		public InlineUploadView(IInlineUploadViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element
		{
			get { return this; }
		}

		private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(e.Uri.ToString());
		}
	}

}
