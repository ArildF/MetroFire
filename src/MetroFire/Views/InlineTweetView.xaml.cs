using System.Windows.Controls;
using System.Windows.Documents;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for InlineTweetView.xaml
	/// </summary>
	public partial class InlineTweetView
	{
		private Tweet _tweet;

		private InlineTweetView()
		{
			InitializeComponent();
		}

		

		public InlineTweetView(Inline body, Tweet tweet) : this()
		{
			_tweet = tweet;
			_body.Inlines.Add(body);

			DataContext = this;
		}

		public string AvatarUrl
		{
			get { return _tweet.AuthorAvatarUrl; }
		}

		public string TwitterUserName
		{
			get { return _tweet.AuthorUsername; }
		}

		public string TwitterProfileUrl
		{
			get { return string.Format("https://twitter.com/#!/{0}", _tweet.AuthorUsername); }
		}

		public string TweetUrl
		{
			get { return string.Format("https://twitter.com/#!/{0}/status/{1}", _tweet.AuthorUsername, _tweet.Id); }
		}
	}
}