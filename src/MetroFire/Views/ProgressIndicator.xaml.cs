using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rogue.MetroFire.UI
{
	/// <summary>
	/// Interaction logic for ProgressIndicator.xaml
	/// </summary>
	public partial class ProgressIndicator : UserControl
	{
		public ProgressIndicator()
		{
			this.InitializeComponent();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == VisibilityProperty)
			{
				var sb = (Storyboard)FindResource("Rotate");
				if (Visibility == Visibility.Visible)
				{
					sb.Begin(this, HandoffBehavior.SnapshotAndReplace, true);
					//BeginStoryboard(sb, HandoffBehavior.SnapshotAndReplace, true);
				}
				else
				{
					sb.Remove(this);
				}
			}
		}

		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			//var sb = (Storyboard)FindResource("Rotate");
			//if (IsVisible)
			//{
			//    BeginStoryboard(sb, HandoffBehavior.SnapshotAndReplace, true);
			//}
			//else
			//{
			//    sb.Stop();
			//}
		}
	}
}