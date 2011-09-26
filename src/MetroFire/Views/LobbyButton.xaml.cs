using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for LobbyButton.xaml
	/// </summary>
	public partial class LobbyButton
	{
		public static readonly DependencyProperty IsJoiningRoomProperty;

		static LobbyButton()
		{
			var md = new FrameworkPropertyMetadata(false, IsJoiningRoomPropertyChanged);
			IsJoiningRoomProperty = DependencyProperty.Register("IsJoiningRoom", typeof(bool), typeof(LobbyButton), md);

		}

		public bool IsJoiningRoom
		{
			get { return (bool)GetValue(IsJoiningRoomProperty); }
			set { SetValue(IsJoiningRoomProperty, value); }
		}


		private static void IsJoiningRoomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var owner = (LobbyButton)d;
			var newValue = (bool)e.NewValue;

			var border = owner.FindVisualChild<Border>("_border");

			bool retval = VisualStateManager.GoToElementState(border, newValue ? "JoiningRoomState" : "NormalState", true);
			Debug.WriteLine("RetVal: " + retval);
		}

		public LobbyButton()
		{
			InitializeComponent();

			DataContextChanged += (sender, args) =>
				{
					var binding = new Binding("IsJoiningRoom") { Mode = BindingMode.OneWay, Source = DataContext };

					SetBinding(IsJoiningRoomProperty, binding);
				};
		}
	}
}