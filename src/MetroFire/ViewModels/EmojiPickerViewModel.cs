using System;
using System.Collections.Generic;
using ReactiveUI;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class EmojiPickerViewModel : ReactiveObject, IEmojiPickerViewModel
	{
		private IMessageBus _bus;
		private readonly IEmojiProvider _emojiProvider;

		private EmojiCategoryViewModel[] _categoryViewModels;
		private EmojiCategoryViewModel _selectedCategory;


		public EmojiPickerViewModel(IMessageBus bus, IEmojiProvider emojiProvider)
		{
			_bus = bus;
			_emojiProvider = emojiProvider;

			EmojiPickedCommand = new ReactiveCommand();

			InsertEmoji = EmojiPickedCommand.OfType<EmojiViewModel>().Select(vm => vm.ShortName);
		}

		public IObservable<string> InsertEmoji { get; set; }

		public ReactiveCommand EmojiPickedCommand { get; private set; }

		public EmojiCategoryViewModel[] EmojiCategories
		{
			get { return _categoryViewModels ?? (_categoryViewModels = InitializeCategoryViewModels()); }
		}

		private EmojiCategoryViewModel[] InitializeCategoryViewModels()
		{
			var assets = _emojiProvider.GetEmojis();

			var grouped = assets.ToLookup(g => g.Emoji.Category);
			var vms = new[]
			{
				new EmojiCategoryViewModel("emoticons", grouped["emoticons"]),
				new EmojiCategoryViewModel("objects", grouped["objects"]),
				new EmojiCategoryViewModel("nature", grouped["nature"]),
				new EmojiCategoryViewModel("places", grouped["places"]), 
				new EmojiCategoryViewModel("other", grouped["other"]), 

			};
			SelectedCategory = vms.FirstOrDefault();
			return vms;
		}

		public EmojiCategoryViewModel SelectedCategory
		{
			get { return _selectedCategory; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedCategory, ref _selectedCategory, value); }
		}

	}

	public class EmojiCategoryViewModel
	{
		public string CategoryTitle { get; private set; }

		public EmojiCategoryViewModel(string categoryTitle, IEnumerable<EmojiAsset> emojiAssets)
		{
			CategoryTitle = categoryTitle;
			Emojis = emojiAssets.OrderBy(a => a.Emoji.CategoryOrder)
				.Select(a => new EmojiViewModel(a)).ToArray();
		}

		public EmojiViewModel[] Emojis
		{
			get; 
			private set; 
		}
	}

	public class EmojiViewModel
	{
		private readonly EmojiAsset _emoji;

		public EmojiViewModel(EmojiAsset emoji)
		{
			_emoji = emoji;
			Keywords = string.Join(", ", _emoji.Emoji.Keywords);
		}

		public string Keywords
		{
			get; private set;
		}

		public object Visual
		{
			get { return _emoji.DrawingGroup; }
		}

		public string Name
		{
			get { return _emoji.Emoji.Name; }
		}

		public string ShortName
		{
			get { return _emoji.Emoji.Shortname; }
		}

		public string Order
		{
			get { return _emoji.Emoji.CategoryOrder.ToString(); }
		}

		public string Unicode
		{
			get { return _emoji.Emoji.Unicode; }
		}
	}
}
