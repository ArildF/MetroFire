using System;
using System.IO;
using System.Xml.Serialization;
using ReactiveUI;

namespace Rogue.MetroFire.UI.Settings
{
	public class SettingsPersistence : ISettingsLoader, ISettingsSaver
	{
		private readonly IMessageBus _bus;

		private readonly object _fileLock = new object();



		public SettingsPersistence(IMessageBus bus)
		{
			_bus = bus;
		}

		public MetroFireSettings Load()
		{
			var serializer = GetSerializer();
			var path = EstablishConfigPath();
			if (!File.Exists(path))
			{
				DoSave(new MetroFireSettings
					{
						General = new GeneralSettings(), 
						Network = new NetworkSettings(), 
						Notification = new NotificationSettings{Notifications = new NotificationEntry[]{}}});
					}
			lock (_fileLock)
			{
				using (var stream = File.OpenRead(path))
				{
					return (MetroFireSettings) serializer.Deserialize(stream);
				}
			}
		}

		public void Save(MetroFireSettings settings)
		{
			DoSave(settings);

			_bus.SendMessage(new SettingsChangedMessage());
		}

		private void DoSave(MetroFireSettings settings)
		{
			var path = EstablishConfigPath();
			var serializer = GetSerializer();
			lock (_fileLock)
			{
				using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					serializer.Serialize(stream, settings);
				}
			}
		}

		private string EstablishConfigPath()
		{
			var path =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create),
					"MetroFire");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			path = Path.Combine(path, "MetroFire.xml");

			return path;
		}

		private XmlSerializer GetSerializer()
		{
			return new XmlSerializer(typeof(MetroFireSettings));
		}
	}

	public interface ISettingsLoader
	{
		MetroFireSettings Load();
	}

	public interface ISettingsSaver
	{
		void Save(MetroFireSettings settings);
	}
}
