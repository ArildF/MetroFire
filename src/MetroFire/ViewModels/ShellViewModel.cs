namespace Rogue.MetroFire.UI.ViewModels
{
	public class ShellViewModel : IShellViewModel
	{
		public ShellViewModel()
		{
			Title = "metro fire";
		}
		public string Title { get; private set; }
	}
}
