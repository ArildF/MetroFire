using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Rogue.MetroFire.UI.Infrastructure;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class PostAsLanguageViewModel : ReactiveObject
	{
		private LanguageOption _selectedLanguageOption;

		public PostAsLanguageViewModel()
		{
			LanguageOptions = new List<LanguageOption>
			{
				new LanguageOption("None (plain text)", ""),
				new LanguageOption("C#", "c#"),
				new LanguageOption("F#", "f#"),
				new LanguageOption("SQL", "sql"),
				new LanguageOption("XML", "xml"),
				new LanguageOption("C++", "cpp"),
				new LanguageOption("CSS", "css"),
				new LanguageOption("Haskell", "haskell"),
				new LanguageOption("Java", "java"),
				new LanguageOption("Javascript", "javascript"),
				new LanguageOption("TypeScript", "typescript"),
				new LanguageOption("Powershell", "powershell"),
				new LanguageOption("HTML", "html"),
				new LanguageOption("PHP", "php"),
				new LanguageOption("VB.NET", "vb.net"),
				new LanguageOption("Markdown", "markdown"),
			};

			SelectedLanguageOption = LanguageOptions.First();
		}
			
		public LanguageOption SelectedLanguageOption
		{
			get { return _selectedLanguageOption;}
			set { this.RaiseAndSetIfChanged(vm => vm.SelectedLanguageOption, ref _selectedLanguageOption, value); }
		}

		public List<LanguageOption> LanguageOptions { get; private set; }

		public class LanguageOption
		{
			public LanguageOption(string displayText, string value)
			{
				DisplayText = displayText;
				Value = value;
			}

			public string DisplayText { get; private set; }
			public string Value { get; private set; }
		}

		public string Process(string userMessage)
		{
			if (SelectedLanguageOption == LanguageOptions.First())
			{
				return userMessage;
			}
			var languageString = "!!L=" + SelectedLanguageOption.Value;
			var nonPrintableLanguageString = UnicodeConvert.ToNonPrintableString(languageString);

			return nonPrintableLanguageString + userMessage;
		}
	}
}
