using System.Windows.Media;
using ReactiveUI;
using Rogue.MetroFire.UI.Settings;
using System.Linq;

namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class HighlightTextViewModel : ReactiveObject, IActionSubViewModel
	{
		private readonly HighlightTextNotificationAction _notificationAction;
		private ComboViewModel<Color> _selectedColor;

		public HighlightTextViewModel(HighlightTextNotificationAction notificationAction)
		{
			_notificationAction = notificationAction;


			AvailableColors = new[]
			{
				new ComboViewModel<Color>("Red", Colors.Red), 
				new ComboViewModel<Color>("Gray", Colors.Gray),
				new ComboViewModel<Color>("Blue", Colors.Blue), 
				new ComboViewModel<Color>("Green", Colors.Green), 
				new ComboViewModel<Color>("White", Colors.White), 
				new ComboViewModel<Color>("Purple", Colors.Purple), 
				new ComboViewModel<Color>("Yellow", Colors.Yellow), 
			};

			SelectedColor = AvailableColors.FirstOrDefault(c => 
				c.Data.Equals(_notificationAction.Color)) ?? AvailableColors.First();
		}

		public ComboViewModel<Color>[] AvailableColors { get; private set; }

		public ComboViewModel<Color> SelectedColor
		{
			get { return _selectedColor; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedColor, ref _selectedColor, value); }
		}

		public void Commit()
		{
			_notificationAction.Color = SelectedColor != null ? SelectedColor.Data : default(Color);
		}

		public Color Color { get { return SelectedColor.Data; } }
	}
}
