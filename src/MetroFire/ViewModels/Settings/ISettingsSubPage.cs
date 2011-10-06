namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public interface ISettingsSubPage
	{
		string Title { get; }
		void Save();
	}
}