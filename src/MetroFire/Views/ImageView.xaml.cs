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

			WindowState = WindowState.Maximized;

		}

		public ImageView(string file) : this()
		{
			_image.Source = new BitmapImage(new Uri(file));
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			Close();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Escape)
			{
				Close();
			}
		}
	}

	
}
