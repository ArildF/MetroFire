using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class EmojiPickerViewModel : ReactiveObject, IEmojiPickerViewModel
	{
		private readonly IEmojiProvider _emojiProvider;

		private readonly ReactiveCollection<EmojiCategoryViewModel> _categoryViewModels;
		private EmojiCategoryViewModel _selectedCategory;
		private string _searchText;
		private readonly ObservableAsPropertyHelper<bool> _userTypedSearch;
		private readonly EmojiViewModel[] _allEmojiViewModels;
		private readonly EmojiCategoryViewModel _searchCategoryViewModel;
		private EmojiCategoryViewModel _previousCategory;


		public EmojiPickerViewModel(IEmojiProvider emojiProvider)
		{
			_emojiProvider = emojiProvider;

			EmojiPickedCommand = new ReactiveCommand();

			InsertEmoji = EmojiPickedCommand.Select(_ => SelectedCategory)
				.Where(c => c != null)
				.Select(c =>c.SelectedEmoji)
				.Where(e => e != null).Do(_ => SearchText = String.Empty).Select(vm => vm.ShortName);

			var searchTextObservable = this.ObservableForProperty(vm => vm.SearchText);
			_userTypedSearch = searchTextObservable
				.Select(_ => !String.IsNullOrEmpty(SearchText))
				.ToProperty(this, vm => vm.UserTypedSearch);

			_categoryViewModels = InitializeCategoryViewModels();
			_searchCategoryViewModel = new EmojiCategoryViewModel("re_sults", Enumerable.Empty<EmojiAsset>());

			_allEmojiViewModels = _categoryViewModels.SelectMany(c => c.Emojis).ToArray();

			var searchText = searchTextObservable.DistinctUntilChanged().Throttle(TimeSpan.FromMilliseconds(300))
				.Select(_ => SearchText);

			searchText.Where(t => !String.IsNullOrEmpty(t)).Where(t => t.Length > 1).Select(
				t => _allEmojiViewModels.Select(e => new {SearchText, Emoji = e})
					.Select(e => new {e.Emoji, Score = Score(e.SearchText, e.Emoji)})
					.Where(e => e.Score > 0)
					.OrderByDescending(e => e.Score)
					.Select(e => e.Emoji).ToArray()
				)
				.SubscribeUI(SetSearchResults);

			searchText.Where(String.IsNullOrEmpty).SubscribeUI(_ =>
			{
				SelectedCategory = _previousCategory;
				EmojiCategories.Remove(_searchCategoryViewModel);
			});

			FocusSelectedCommand = new ReactiveCommand();
			FocusSelectedCommand.Subscribe(_ => FocusSelected());

		}

		public ReactiveCommand FocusSelectedCommand { get; private set; }

		private void SetSearchResults(EmojiViewModel[] res)
		{
			if (!_categoryViewModels.Contains(_searchCategoryViewModel))
			{
				_previousCategory = SelectedCategory;
				_categoryViewModels.Add(_searchCategoryViewModel);
			}

			_searchCategoryViewModel.Emojis = res;

			SelectedCategory = _searchCategoryViewModel;
			SelectedCategory.SelectedEmoji = SelectedCategory.Emojis.FirstOrDefault();
		}


		private int Score(string searchText, EmojiViewModel emoji)
		{
			return (int)(ScoreString(emoji.Name, searchText, 10.0) +
				ScoreString(emoji.ShortName, searchText, 7.5) +
				ScoreString(emoji.Unicode, searchText, 5.0) +
				emoji.IndividualKeywords.Select(kw => ScoreString(kw, searchText, 8.0)).Sum());
		}

		private double ScoreString(string attribute, string searchText, double weight)
		{
			var index = attribute.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
			if (index == -1)
			{
				return 0;
			}
			return index == 0 ? weight*1.25 : weight;
		}

		public bool UserTypedSearch { get { return _userTypedSearch.Value; }}

		public IObservable<string> InsertEmoji { get; set; }
		public void FocusSelected()
		{
			if (SelectedCategory != null)
			{
				SelectedCategory.Focus();
			}
		}

		public string SearchText
		{
			get { return _searchText; }
			set { this.RaiseAndSetIfChanged(vm => vm.SearchText, ref _searchText, value); }
		}

		public ReactiveCommand EmojiPickedCommand { get; private set; }

		public ReactiveCollection<EmojiCategoryViewModel> EmojiCategories
		{
			get { return _categoryViewModels; }
		}

		private ReactiveCollection<EmojiCategoryViewModel> InitializeCategoryViewModels()
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
			return new ReactiveCollection<EmojiCategoryViewModel>(vms);
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
		private EmojiViewModel[] _emoji;

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
			get { return _emoji; }
			set { this.RaiseAndSetIfChanged(vm => vm.Emojis, ref _emoji, value); }
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

		public string[] IndividualKeywords
		{
			get { return _emoji.Emoji.Keywords; }
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
