using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Castle.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using Rogue.MetroFire.CampfireClient;
using Rogue.MetroFire.UI.Assets;
using Rogue.MetroFire.UI.Properties;

namespace Rogue.MetroFire.UI.Infrastructure
{
	public class EmojiLoader : IStartable, IEmojiProvider
	{
		private readonly IMessageBus _bus;

		private readonly ManualResetEvent _waitEvent = new ManualResetEvent(false);
		private EmojiAsset[] _emojis;

		public EmojiLoader(IMessageBus bus)
		{
			_bus = bus;
		}

		private void SetEmojis(EmojiAsset[] value)
		{
			_waitEvent.Set();
			_emojis = value;
		}

		public EmojiAsset[] GetEmojis()
		{
			_waitEvent.WaitOne();
			return _emojis;
		}

		public void Start()
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					var emojis = LoadEmojiFromResources();
					SetEmojis(JoinWithGraphicsFromResourceDictionary(emojis));
				}
				catch (Exception e)
				{
					_bus.SendMessage(new ExceptionMessage(e));
				}
			});
		}

		private EmojiAsset[] JoinWithGraphicsFromResourceDictionary(IEnumerable<Emoji> emojis)
		{
			var resources = new ResourceDictionary
			{
				Source = new Uri(@"pack://application:,,,/Assets/EmojiResourceDictionary.xaml")
			};
			var joinedEmojis = (from e in emojis
			                    let drawingName = "emoji_" + e.Key
			                    where resources.Contains(drawingName)
			                    let drawing = resources[drawingName] as DrawingGroup
			                    select new EmojiAsset(e, drawing)).ToArray();

			foreach (var joinedEmoji in joinedEmojis)
			{
				joinedEmoji.DrawingGroup.Freeze();
			}

			return joinedEmojis;
		}

		private IEnumerable<Emoji> LoadEmojiFromResources()
		{
			var emojis = JsonConvert.DeserializeObject<Dictionary<string, Emoji>>(
				Resources.emoji, new JsonSerializerSettings
				{
					ContractResolver = new UnderscoresPropertyNamesContractResolver()
				});

			foreach (var e in emojis)
			{
				e.Value.Key = e.Key;
			}

			return emojis.Values;
		}

		private class UnderscoresPropertyNamesContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				var humps = SplitByHumps(propertyName);

				return string.Join("_", humps).ToLower(CultureInfo.InvariantCulture);

			}

			private IEnumerable<string> SplitByHumps(string propertyName)
			{
				if (String.IsNullOrEmpty(propertyName))
				{
					yield break;
				}

				int start = 0;

				for (int i = 1; i < propertyName.Length; i++)
				{
					if (Char.IsUpper(propertyName[i]))
					{
						yield return propertyName.Substring(start, i - start);
						start = i;
					}
				}
				if (start != propertyName.Length)
				{
					yield return propertyName.Substring(start, propertyName.Length - start);
				}
			}
		}

		public void Stop()
		{
		}
	}


}
