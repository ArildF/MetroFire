using System.Windows.Controls;
using System.Windows.Documents;

namespace Rogue.MetroFire.UI.Views
{
	/// <summary>
	/// Interaction logic for InlineTweetView.xaml
	/// </summary>
	public partial class InlineTweetView
	{
		private readonly string _user;
		private readonly string _tweetUrl;

		private InlineTweetView()
		{
			InitializeComponent();
		}

		public InlineTweetView(string user, Inline body, string tweetUrl) : this()
		{
			_user = user;
			_tweetUrl = tweetUrl;
			_body.Inlines.Add(body);

			DataContext = this;
		}

		public string AvatarUrl
		{
			get { return string.Format("http://api.twitter.com/1/users/profile_image?screen_name={0}", _user); }
		}

		public string TwitterUserName
		{
			get { return _user; }
		}

		public string TwitterProfileUrl
		{
			get { return string.Format("https://twitter.com/#!/{0}", _user); }
		}

		public string TweetUrl
		{
			get { return _tweetUrl; }
		}
	}
}