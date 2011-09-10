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

	public class ActivateModuleMessage
	{
		public string ParentModule { get; private set; }

		public IModule Module { get; private set; }

		public ActivateModuleMessage(string parentModule, IModule module)
		{
			ParentModule = parentModule;
			Module = module;
		}
	}

	public class ActivateModuleByIdMessage
	{
		public string ParentModule { get; private set; }
		public int Id { get; private set; }

		public ActivateModuleByIdMessage(string parentModule, int id)
		{
			ParentModule = parentModule;
			Id = id;
		}
	}

	public class ModuleLoaded
	{
		public string ModuleName { get; private set; }

		public ModuleLoaded(string moduleName)
		{
			ModuleName = moduleName;
		}
	}

	public class RoomModuleCreatedMessage
	{
		public IModule Module { get; private set; }

		public RoomModuleCreatedMessage(IModule module)
		{
			Module = module;
		}
	}


}
