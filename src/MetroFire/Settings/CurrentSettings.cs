using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using System;

namespace Rogue.MetroFire.UI.Settings
{
	public class CurrentSettings : ISettings, CampfireClient.ISettings
	{
		private readonly ISettingsLoader _loader;
		private readonly IMessageBus _bus;
		private MetroFireSettings _settings;

		public CurrentSettings(ISettingsLoader loader, IMessageBus bus)
		{
			_loader = loader;
			_bus = bus;

			_settings = _loader.Load();

			_bus.Listen<SettingsChangedMessage>().Subscribe(_ => _settings = _loader.Load());
		}

		public INetworkSettings Network
		{
			get { return _settings.Network; }
		}
	}
}
