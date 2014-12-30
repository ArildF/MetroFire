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
				new LanguageOption("C#", "C#"),
				new LanguageOption("SQL", "SQL"),
				new LanguageOption("XML", "XML"),
				new LanguageOption("C++", "C++"),
				new LanguageOption("Haskell", "Haskell"),
				new LanguageOption("Java", "Java"),
				new LanguageOption("Javascript", "JavaScript"),
				new LanguageOption("HTML", "HTML"),
				new LanguageOption("MSIL", "MSIL"),
				new LanguageOption("COBOL", "COBOL"),
				new LanguageOption("Eiffel", "Eiffel"),
				new LanguageOption("Pascal", "Pascal"),
				new LanguageOption("Perl", "Perl"),
				new LanguageOption("PHP", "PHP"),
				new LanguageOption("Python", "Python"),
				new LanguageOption("Ruby", "Ruby"),
				new LanguageOption("VBScript", "VBScript"),
				new LanguageOption("VB.NET", "VB.NET"),
				new LanguageOption("Fortran", "Fortran"),
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
