using System.Collections.Generic;
using Rogue.MetroFire.CampfireClient.Serialization;

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

	public class UsersUpdatedMessage
	{
		public IEnumerable<User> UsersToUpdate { get; private set; }

		public UsersUpdatedMessage(IEnumerable<User> usersToUpdate)
		{
			UsersToUpdate = usersToUpdate;
		}
	}


	public class RoomActivatedMessage
	{
		public int RoomId { get; private set; }

		public RoomActivatedMessage(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class RoomDeactivatedMessage
	{
		public int RoomId { get; private set; }

		public RoomDeactivatedMessage(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class RoomActivityMessage
	{
		public int RoomId { get; private set; }
		public int Count { get; private set; }

		public RoomActivityMessage(int roomId, int count)
		{
			RoomId = roomId;
			Count = count;
		}
	}

	public class ApplicationActivatedMessage{}
	public class ApplicationDeactivatedMessage {}

	public class NavigateMainModuleMessage
	{
		public string ModuleName { get; private set; }

		public NavigateMainModuleMessage(string moduleName)
		{
			ModuleName = moduleName;
		}
	}

	public class NavigateBackMainModuleMessage
	{
	}

	public class SettingsChangedMessage{}

}
