using System;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.UI.GitHub;
using System.Reactive.Linq;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class GitHubCommitsViewModel : ReactiveObject
	{
		private Commit[] _commits;

		public GitHubCommitsViewModel(IGitHubClient client, IWebBrowser browser)
		{

			NavigateCommand = new ReactiveCommand();
			NavigateCommand.OfType<string>().Subscribe(url => browser.NavigateTo(new Uri(url)));


			var cmd = new ReactiveAsyncCommand();
			var obs = cmd.RegisterAsyncFunction(_ => client.GetLatestCommits());
			obs.Subscribe(commits => Commits = commits);

			Observable.Interval(TimeSpan.FromHours(3)).Subscribe(_ => cmd.Execute(null));

			cmd.Execute(null);
		}

		public ReactiveCommand NavigateCommand
		{
			get; private set; 
		}

		public Commit[] Commits
		{
			get { return _commits; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Commits, ref _commits, value); }
		}
		
	}
}
