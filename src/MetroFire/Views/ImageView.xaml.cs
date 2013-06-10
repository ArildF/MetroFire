using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for ImageView.xaml
	/// </summary>
	public partial class ImageView : IImageView
	{
		private ImageView()
		{
			InitializeComponent();

			SourceInitialized += (o, e) => WindowState = WindowState.Maximized;
		}

		public ImageView(string file) : this()
		{
			Image.Uri = file;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Escape)
			{
				Close();
			}
		}

		private void GridOnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Close();
		}
	}

	
}
