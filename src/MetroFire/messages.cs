namespace Rogue.MetroFire.UI
{
	public class ApplicationLoadedMessage
	{}

	public class ActivateMainModuleMessage
	{
		public string ModuleName { get; private set; }

		public ActivateMainModuleMessage(string moduleName)
		{
			ModuleName = moduleName;
		}
	}
}
