namespace Rogue.MetroFire.UI.ViewModels.Settings
{
	public class ComboViewModel<T>
	{
		public string Text { get; private set; }
		public T Data { get; private set; } 

		public ComboViewModel(string text, T data)
		{
			Text = text;
			Data = data;
		}
	}
}