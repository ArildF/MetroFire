using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.CampfireClient.Serialization;
using Rogue.MetroFire.UI.Infrastructure;
using Rogue.MetroFire.UI.Settings;

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

	public class ModuleActivatedMessage
	{
		public IModule Module { get; private set; }
		public string ParentModule { get; private set; }

		public ModuleActivatedMessage(IModule module, string parentModule)
		{
			Module = module;
			ParentModule = parentModule;
		}
	}

	public class NewMessagesReceivedMessage
	{
		public IEnumerable<Message> Messages { get; private set; }

		public int RoomId { get; private set; }

		public NewMessagesReceivedMessage(IEnumerable<Message> messages, int roomId)
		{
			Messages = messages;
			RoomId = roomId;
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

	public class NavigateSettingsPageMessage
	{
		public string SettingsPage { get; private set; }

		public NavigateSettingsPageMessage(string settingsPage)
		{
			SettingsPage = settingsPage;
		}
	}

	public class NotificationMessage
	{
		public IRoom Room { get; private set; }
		public User User { get; private set; }
		public Message Message { get; private set; }

		public NotificationMessage(IRoom room, User user, Message message)
		{
			Room = room;
			User = user;
			Message = message;
		}
	}

	public class RoomBackLogLoadedMessage
	{
		public int RoomId { get; private set; }

		public RoomBackLogLoadedMessage(int roomId)
		{
			RoomId = roomId;
		}
	}

	public class PasteImageMessage
	{
		public IRoom Room { get; private set; }
		public BitmapSource Image { get; private set; }

		public PasteImageMessage(IRoom room, BitmapSource image)
		{
			Room = room;
			Image = image;
		}
	}

	public class ShowToastMessage
	{
		public NotificationMessage Message { get; private set; }
		public ShowToastNotificationAction Action { get; private set; }

		public ShowToastMessage(NotificationMessage message, ShowToastNotificationAction action)
		{
			Message = message;
			Action = action;
		}
	}

	public class AppUpdateProgressMessage
	{
		public int ProgressPercentage { get; private set; }

		public AppUpdateProgressMessage(int progressPercentage)
		{
			ProgressPercentage = progressPercentage;
		}
	}

	public class AppUpdateAvailableMessage
	{
	}

	public class RequestAppUpdateMessage
	{
	}

	public class RequestApplicationRestartMessage
	{
	}

	public class RequestUploadFilePickerMessage : CorrelatedMessage
	{
		
	}

	public class UploadFilePickedMessage : CorrelatedReply
	{
		public FileItem FileItem { get; set; }

		public UploadFilePickedMessage(FileItem item, Guid correlationId) : base(correlationId)
		{
			FileItem = item;
		}
	}
}
