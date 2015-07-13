using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.ViewModels
{
	public class InlineTweetViewModel : ReactiveObject
	{
		private string _document;

		public InlineTweetViewModel(ITwitterClient twitterClient, Tweet tweet)
		{
			var cmd = new ReactiveAsyncCommand();
			var obs = cmd.RegisterAsyncFunction(_ => twitterClient.GetTweet(tweet.Id).Html);

			obs.SubscribeUI(html => Document = 
				html, ex => Document = ex.ToString());
			cmd.Execute(null);
		}

		public string Document
		{
			get { return _document; }
			private set { this.RaiseAndSetIfChanged(vm => vm.Document, ref _document, value); }
		}
	}
}
