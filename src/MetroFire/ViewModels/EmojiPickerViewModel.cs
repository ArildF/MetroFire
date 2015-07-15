using System;
using System.Collections.Generic;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

			InsertEmoji = EmojiPickedCommand.Select(_ => SelectedCategory)
				.Where(c => c != null)
				.Select(c =>c.SelectedEmoji)
				.Where(e => e != null).Select(vm => vm.ShortName);
		}

		public IObservable<string> InsertEmoji { get; set; }
		public void Focus()
		{
			if (SelectedCategory != null)
			{
				SelectedCategory.Focus();
			}
		}

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
				new EmojiCategoryViewModel("_emoticons", grouped["emoticons"]),
				new EmojiCategoryViewModel("_objects", grouped["objects"]),
				new EmojiCategoryViewModel("_nature", grouped["nature"]),
				new EmojiCategoryViewModel("_places", grouped["places"]), 
				new EmojiCategoryViewModel("o_ther", grouped["other"]), 

			};
			SelectedCategory = vms.FirstOrDefault();
			if (SelectedCategory != null)
			{
				SelectedCategory.SelectedEmoji = SelectedCategory.Emojis.FirstOrDefault();
			}
			return vms;
		}

		public EmojiCategoryViewModel SelectedCategory
		{
			get { return _selectedCategory; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedCategory, ref _selectedCategory, value); }
		}

	}

	public class EmojiCategoryViewModel : ReactiveObject
	{
		private EmojiViewModel _selectedEmoji;
		public string CategoryTitle { get; private set; }

		private readonly Subject<Unit> _focus = new Subject<Unit>();

		public EmojiCategoryViewModel(string categoryTitle, IEnumerable<EmojiAsset> emojiAssets)
		{
			CategoryTitle = categoryTitle;
			Emojis = emojiAssets.OrderBy(a => a.Emoji.CategoryOrder)
				.Select(a => new EmojiViewModel(a)).ToArray();
		}

		public EmojiViewModel SelectedEmoji
		{
			get { return _selectedEmoji; }
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedEmoji, ref _selectedEmoji, value); }
		}

		public IObservable<Unit> FocusTo
		{
			get { return _focus; }
		}

		public EmojiViewModel[] Emojis
		{
			get; 
			private set; 
		}

		public void Focus()
		{
			_focus.OnNext(Unit.Default);
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
