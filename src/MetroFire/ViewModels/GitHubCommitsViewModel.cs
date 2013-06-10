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

		public GitHubCommitsViewModel(IGitHubClient client)
		{
			var cmd = new ReactiveAsyncCommand();
			var obs = cmd.RegisterAsyncFunction(_ => client.GetLatestCommits());
			obs.Subscribe(commits => Commits = commits);

			Observable.Interval(TimeSpan.FromHours(3)).Subscribe(_ => cmd.Execute(null));

			cmd.Execute(null);
		}

		public Commit[] Commits
		{
			get { return _commits; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Commits, ref _commits, value); }
		}
	}
}
