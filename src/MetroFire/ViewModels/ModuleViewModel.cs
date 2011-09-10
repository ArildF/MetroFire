namespace Rogue.MetroFire.UI.ViewModels
{
	public class ModuleViewModel
	{
		private readonly IModule _module;

		public ModuleViewModel(IModule module)
		{
			_module = module;
		}

		public IModule Module { get { return _module; } }

		public bool IsActive { get { return _module.IsActive; } }
		public string Caption { get { return _module.Caption; } }
	}
}