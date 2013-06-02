using System.IO;
using Newtonsoft.Json;
using Rogue.MetroFire.UI.GitHub;

namespace Rogue.MetroFire.UI.SampleData
{
	public class SampleGitHubCommitsViewModel
	{
		public SampleGitHubCommitsViewModel()
		{
			var serializer = new JsonSerializer();
			Commits = serializer.Deserialize<Commit[]>(new JsonTextReader(new StringReader(Json)));
		}

		public Commit[] Commits
		{
			get; set;
		}

		private const string Json = @"[
  {
    'sha': '6e6d5da2e1e8e5c6ab691f5b1b6a57b846fe2f89',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-06-01T11:29:46Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-06-01T11:29:46Z'
      },
      'message': 'Massive rearchitecture\n\nMove stuff out of view code and into viewmodels.',
      'tree': {
        'sha': '8ef3960d5d6dc8aff6d126d74465315a31d304a1',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/8ef3960d5d6dc8aff6d126d74465315a31d304a1'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/6e6d5da2e1e8e5c6ab691f5b1b6a57b846fe2f89',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/6e6d5da2e1e8e5c6ab691f5b1b6a57b846fe2f89',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/6e6d5da2e1e8e5c6ab691f5b1b6a57b846fe2f89',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/6e6d5da2e1e8e5c6ab691f5b1b6a57b846fe2f89/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329'
      }
    ]
  },
  {
    'sha': 'a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-24T19:01:13Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-24T19:01:13Z'
      },
      'message': 'Show an icon overlay indicating the number of unread messages.',
      'tree': {
        'sha': 'a4fc9bb93ee5ec9c0bcc4facb01ac4fa1c1bb267',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/a4fc9bb93ee5ec9c0bcc4facb01ac4fa1c1bb267'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a7ad92eabc111d783ab68a6fbf844ebc8f2329/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'f9448013d6e81b51e3338d10f52e1859d3ef85f4',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f9448013d6e81b51e3338d10f52e1859d3ef85f4',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/f9448013d6e81b51e3338d10f52e1859d3ef85f4'
      }
    ]
  },
  {
    'sha': 'f9448013d6e81b51e3338d10f52e1859d3ef85f4',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-24T17:24:11Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-24T17:24:11Z'
      },
      'message': 'Avoid clipping the drop shadow and make the lobby buttons bigger.',
      'tree': {
        'sha': '66bf3b4d97f57b9fe20490151c12732ae88b197f',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/66bf3b4d97f57b9fe20490151c12732ae88b197f'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/f9448013d6e81b51e3338d10f52e1859d3ef85f4',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f9448013d6e81b51e3338d10f52e1859d3ef85f4',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/f9448013d6e81b51e3338d10f52e1859d3ef85f4',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f9448013d6e81b51e3338d10f52e1859d3ef85f4/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '2bf5ae494002fee5797decbb95ce68891b78ceab',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2bf5ae494002fee5797decbb95ce68891b78ceab',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/2bf5ae494002fee5797decbb95ce68891b78ceab'
      }
    ]
  },
  {
    'sha': '2bf5ae494002fee5797decbb95ce68891b78ceab',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T21:10:43Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T21:10:43Z'
      },
      'message': 'Change the lobby layout.',
      'tree': {
        'sha': 'fc7c442a9587f0a50956e1b07e3a94818f8b61c6',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/fc7c442a9587f0a50956e1b07e3a94818f8b61c6'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/2bf5ae494002fee5797decbb95ce68891b78ceab',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2bf5ae494002fee5797decbb95ce68891b78ceab',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/2bf5ae494002fee5797decbb95ce68891b78ceab',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2bf5ae494002fee5797decbb95ce68891b78ceab/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/2aee91767cb8fd6870c0e7c5066b23ff3e69243d'
      }
    ]
  },
  {
    'sha': '2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T21:04:19Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T21:04:19Z'
      },
      'message': 'Font changes to the lobby.',
      'tree': {
        'sha': 'e0ee682762cc407b5d6d2825b17a985a079ec9d6',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/e0ee682762cc407b5d6d2825b17a985a079ec9d6'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/2aee91767cb8fd6870c0e7c5066b23ff3e69243d',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/2aee91767cb8fd6870c0e7c5066b23ff3e69243d/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '82adcf61c3d7b79897651c5ed89c934380ab983f',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/82adcf61c3d7b79897651c5ed89c934380ab983f',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/82adcf61c3d7b79897651c5ed89c934380ab983f'
      }
    ]
  },
  {
    'sha': '82adcf61c3d7b79897651c5ed89c934380ab983f',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T20:18:54Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T20:18:54Z'
      },
      'message': 'Use a variety of colors on the room buttons in the lobby',
      'tree': {
        'sha': 'e1f5f5a0665337ef0d197bf30f7ac40f7fe5f820',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/e1f5f5a0665337ef0d197bf30f7ac40f7fe5f820'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/82adcf61c3d7b79897651c5ed89c934380ab983f',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/82adcf61c3d7b79897651c5ed89c934380ab983f',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/82adcf61c3d7b79897651c5ed89c934380ab983f',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/82adcf61c3d7b79897651c5ed89c934380ab983f/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17'
      }
    ]
  },
  {
    'sha': '3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T19:46:26Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T19:46:26Z'
      },
      'message': ""Ensure there's a vertical scrollbar when you have many rooms"",
      'tree': {
        'sha': '7aa127d890b14b00004b46c0316584f384b257d5',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/7aa127d890b14b00004b46c0316584f384b257d5'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3f73b5d3f228f5e90eecd0cd05beeaeb87ddcd17/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'a4967b776ec27c1031d7b65449de960c01b68f8a',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4967b776ec27c1031d7b65449de960c01b68f8a',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/a4967b776ec27c1031d7b65449de960c01b68f8a'
      }
    ]
  },
  {
    'sha': 'a4967b776ec27c1031d7b65449de960c01b68f8a',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T18:54:04Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-22T18:54:04Z'
      },
      'message': 'Add a button for uploading files.',
      'tree': {
        'sha': 'fed7a22c6241801e245b058a57f963b09f62fa22',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/fed7a22c6241801e245b058a57f963b09f62fa22'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/a4967b776ec27c1031d7b65449de960c01b68f8a',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4967b776ec27c1031d7b65449de960c01b68f8a',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/a4967b776ec27c1031d7b65449de960c01b68f8a',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4967b776ec27c1031d7b65449de960c01b68f8a/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '6cccbdb0049a337c533a461d3b1948975bc3a9f7',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/6cccbdb0049a337c533a461d3b1948975bc3a9f7',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/6cccbdb0049a337c533a461d3b1948975bc3a9f7'
      }
    ]
  },
  {
    'sha': '6cccbdb0049a337c533a461d3b1948975bc3a9f7',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-21T21:05:40Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-21T21:05:40Z'
      },
      'message': 'Reimplement youtube handling\n\nNow shows small thumbnail which if clicked upon, gives a full-size\nyoutube player.',
      'tree': {
        'sha': '6721fa99cfd82908e8c02fd9b7e15841d9b8cf26',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/6721fa99cfd82908e8c02fd9b7e15841d9b8cf26'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/6cccbdb0049a337c533a461d3b1948975bc3a9f7',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/6cccbdb0049a337c533a461d3b1948975bc3a9f7',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/6cccbdb0049a337c533a461d3b1948975bc3a9f7',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/6cccbdb0049a337c533a461d3b1948975bc3a9f7/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'c07f93ce64c69cdcb20bc0669249debdec43b28b',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/c07f93ce64c69cdcb20bc0669249debdec43b28b',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/c07f93ce64c69cdcb20bc0669249debdec43b28b'
      }
    ]
  },
  {
    'sha': 'c07f93ce64c69cdcb20bc0669249debdec43b28b',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T19:30:10Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T19:30:10Z'
      },
      'message': 'Factor out URL handling to separate classes.',
      'tree': {
        'sha': 'f84fe785e39ef4d106d39fdb2746507da5ee69b9',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/f84fe785e39ef4d106d39fdb2746507da5ee69b9'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/c07f93ce64c69cdcb20bc0669249debdec43b28b',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/c07f93ce64c69cdcb20bc0669249debdec43b28b',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/c07f93ce64c69cdcb20bc0669249debdec43b28b',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/c07f93ce64c69cdcb20bc0669249debdec43b28b/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '94e12adf700b92fd1ea171015c7a8b5f25464847',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/94e12adf700b92fd1ea171015c7a8b5f25464847',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/94e12adf700b92fd1ea171015c7a8b5f25464847'
      }
    ]
  },
  {
    'sha': '94e12adf700b92fd1ea171015c7a8b5f25464847',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T18:49:10Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T18:49:10Z'
      },
      'message': 'Align all text to the left instead of justifying.',
      'tree': {
        'sha': '7abaf942d632aaa8283652e6b419e8beb293960f',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/7abaf942d632aaa8283652e6b419e8beb293960f'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/94e12adf700b92fd1ea171015c7a8b5f25464847',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/94e12adf700b92fd1ea171015c7a8b5f25464847',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/94e12adf700b92fd1ea171015c7a8b5f25464847',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/94e12adf700b92fd1ea171015c7a8b5f25464847/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d'
      }
    ]
  },
  {
    'sha': '3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T18:46:59Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T18:46:59Z'
      },
      'message': 'Initial implementation of inline youtube video rendering.',
      'tree': {
        'sha': 'ca056cdf0915417e50dec5c484a0797b8261087d',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/ca056cdf0915417e50dec5c484a0797b8261087d'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3eb417b982cd2d5f3d57e0cb2c9ea153e4aad17d/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'f8c371033451e593042b3cc89c428ff334f3d0ad',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f8c371033451e593042b3cc89c428ff334f3d0ad',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/f8c371033451e593042b3cc89c428ff334f3d0ad'
      }
    ]
  },
  {
    'sha': 'f8c371033451e593042b3cc89c428ff334f3d0ad',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T17:22:21Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T17:22:21Z'
      },
      'message': 'Add source URL for Play button brush.',
      'tree': {
        'sha': 'a06ebe0abae49c5bac01d1e4567c2128c898444b',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/a06ebe0abae49c5bac01d1e4567c2128c898444b'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/f8c371033451e593042b3cc89c428ff334f3d0ad',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f8c371033451e593042b3cc89c428ff334f3d0ad',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/f8c371033451e593042b3cc89c428ff334f3d0ad',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/f8c371033451e593042b3cc89c428ff334f3d0ad/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04'
      }
    ]
  },
  {
    'sha': '4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T17:16:37Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T17:16:37Z'
      },
      'message': 'Make the initial update check after 15 seconds.',
      'tree': {
        'sha': 'ff61828d5eeae29ebf6faad50a281e56dc6c8566',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/ff61828d5eeae29ebf6faad50a281e56dc6c8566'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/4bc71dc22de0d48d1fbb8a8ffd3d3b5f017d7c04/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'de2bf9e02dc443f94a373199aec55adb446c64aa',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/de2bf9e02dc443f94a373199aec55adb446c64aa',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/de2bf9e02dc443f94a373199aec55adb446c64aa'
      }
    ]
  },
  {
    'sha': 'de2bf9e02dc443f94a373199aec55adb446c64aa',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T16:45:43Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T16:45:43Z'
      },
      'message': 'Avoid transparency effect in gifs\n\nMake each frame a composite of the existing image and the current frame.',
      'tree': {
        'sha': '4f2adc7aee61a9294eb058cab730e91fe38c746a',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/4f2adc7aee61a9294eb058cab730e91fe38c746a'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/de2bf9e02dc443f94a373199aec55adb446c64aa',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/de2bf9e02dc443f94a373199aec55adb446c64aa',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/de2bf9e02dc443f94a373199aec55adb446c64aa',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/de2bf9e02dc443f94a373199aec55adb446c64aa/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207'
      }
    ]
  },
  {
    'sha': 'a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:33:59Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:33:59Z'
      },
      'message': 'Ensure full screen images still animate.',
      'tree': {
        'sha': 'd45bd7f575de4ae73a21776a139a6a4ebd885a30',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/d45bd7f575de4ae73a21776a139a6a4ebd885a30'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a4a2645bfb6d39d72905e015c3bd4cb3fe6ae207/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef'
      }
    ]
  },
  {
    'sha': '766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:29:31Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:29:31Z'
      },
      'message': 'Use arrow cursor for the Play button.',
      'tree': {
        'sha': 'dba30f3cfbaed6dec939f07c8b73eb205ba9537b',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/dba30f3cfbaed6dec939f07c8b73eb205ba9537b'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/766ea3b22c8edb7bc9bc2f885d4c06d4e430c2ef/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '9b6ed104136ba81a5df959104501194dcb727d0b',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/9b6ed104136ba81a5df959104501194dcb727d0b',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/9b6ed104136ba81a5df959104501194dcb727d0b'
      }
    ]
  },
  {
    'sha': '9b6ed104136ba81a5df959104501194dcb727d0b',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:28:03Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:28:03Z'
      },
      'message': 'Remove unused property.',
      'tree': {
        'sha': '4b1ec7d11b8b99d9810ab88cec8209e9cc11aadb',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/4b1ec7d11b8b99d9810ab88cec8209e9cc11aadb'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/9b6ed104136ba81a5df959104501194dcb727d0b',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/9b6ed104136ba81a5df959104501194dcb727d0b',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/9b6ed104136ba81a5df959104501194dcb727d0b',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/9b6ed104136ba81a5df959104501194dcb727d0b/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'a9d09914637715ccbf98b30ffeb513cea0a37db1',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a9d09914637715ccbf98b30ffeb513cea0a37db1',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/a9d09914637715ccbf98b30ffeb513cea0a37db1'
      }
    ]
  },
  {
    'sha': 'a9d09914637715ccbf98b30ffeb513cea0a37db1',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:26:25Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T15:26:25Z'
      },
      'message': 'Allow user to stop and restart animations',
      'tree': {
        'sha': 'b562e2e60356feb2eafd5fe17d37b7d6f3e2b310',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/b562e2e60356feb2eafd5fe17d37b7d6f3e2b310'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/a9d09914637715ccbf98b30ffeb513cea0a37db1',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a9d09914637715ccbf98b30ffeb513cea0a37db1',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/a9d09914637715ccbf98b30ffeb513cea0a37db1',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/a9d09914637715ccbf98b30ffeb513cea0a37db1/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '201339e629c900b4c24695a917ed3592f724287c',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/201339e629c900b4c24695a917ed3592f724287c',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/201339e629c900b4c24695a917ed3592f724287c'
      }
    ]
  },
  {
    'sha': '201339e629c900b4c24695a917ed3592f724287c',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T14:10:21Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T14:10:21Z'
      },
      'message': 'Allow the animation time for GIFs to be configurable.',
      'tree': {
        'sha': '17b30c5dec47cd3916b0ddc2b7d905f2b0713c82',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/17b30c5dec47cd3916b0ddc2b7d905f2b0713c82'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/201339e629c900b4c24695a917ed3592f724287c',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/201339e629c900b4c24695a917ed3592f724287c',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/201339e629c900b4c24695a917ed3592f724287c',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/201339e629c900b4c24695a917ed3592f724287c/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '1a86e09bb12f4b61feb34929042497d839d13982',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1a86e09bb12f4b61feb34929042497d839d13982',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/1a86e09bb12f4b61feb34929042497d839d13982'
      }
    ]
  },
  {
    'sha': '1a86e09bb12f4b61feb34929042497d839d13982',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T13:44:09Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-20T13:44:09Z'
      },
      'message': 'Stop animated GIFs after 60 seconds',
      'tree': {
        'sha': '53319186fdcdf8e341667ce0a5cecc5c99d8e02c',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/53319186fdcdf8e341667ce0a5cecc5c99d8e02c'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/1a86e09bb12f4b61feb34929042497d839d13982',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1a86e09bb12f4b61feb34929042497d839d13982',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/1a86e09bb12f4b61feb34929042497d839d13982',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1a86e09bb12f4b61feb34929042497d839d13982/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e'
      }
    ]
  },
  {
    'sha': '27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T20:40:41Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T20:40:41Z'
      },
      'message': ""Merge branch 'develop' of github.com:ArildF/MetroFire into develop"",
      'tree': {
        'sha': '5e38e6246ee0e6439ede7af924db042a20130820',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/5e38e6246ee0e6439ede7af924db042a20130820'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/27cfea3e4cf84bf891f93dfb0419ed4bcbdf4a1e/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'ad814b0d6da64ccfd2918db13f320e841f5fca9e',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/ad814b0d6da64ccfd2918db13f320e841f5fca9e',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/ad814b0d6da64ccfd2918db13f320e841f5fca9e'
      },
      {
        'sha': '1cebf0b5f13f974d66d1c474a471fb95f9f92030',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1cebf0b5f13f974d66d1c474a471fb95f9f92030',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/1cebf0b5f13f974d66d1c474a471fb95f9f92030'
      }
    ]
  },
  {
    'sha': 'ad814b0d6da64ccfd2918db13f320e841f5fca9e',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T20:40:01Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T20:40:01Z'
      },
      'message': 'Added a user agent.',
      'tree': {
        'sha': 'f4542deb30354e02b1e0a13fe6f6f0832eddf032',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/f4542deb30354e02b1e0a13fe6f6f0832eddf032'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/ad814b0d6da64ccfd2918db13f320e841f5fca9e',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/ad814b0d6da64ccfd2918db13f320e841f5fca9e',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/ad814b0d6da64ccfd2918db13f320e841f5fca9e',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/ad814b0d6da64ccfd2918db13f320e841f5fca9e/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f'
      }
    ]
  },
  {
    'sha': '1cebf0b5f13f974d66d1c474a471fb95f9f92030',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2012-12-30T23:01:26Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T19:20:21Z'
      },
      'message': 'Show image links directly inline',
      'tree': {
        'sha': '8d3644c4bbf161750162617243c5ef4d881164ce',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/8d3644c4bbf161750162617243c5ef4d881164ce'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/1cebf0b5f13f974d66d1c474a471fb95f9f92030',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1cebf0b5f13f974d66d1c474a471fb95f9f92030',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/1cebf0b5f13f974d66d1c474a471fb95f9f92030',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/1cebf0b5f13f974d66d1c474a471fb95f9f92030/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'b8c8335d951789d0450499e6751d582591a3cca1',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b8c8335d951789d0450499e6751d582591a3cca1',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/b8c8335d951789d0450499e6751d582591a3cca1'
      }
    ]
  },
  {
    'sha': 'b8c8335d951789d0450499e6751d582591a3cca1',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2012-12-30T23:01:03Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-05-15T19:20:12Z'
      },
      'message': 'Fix non-compiling tests',
      'tree': {
        'sha': '6f33a998fab91afd9ca0f344c9eddd862aedc4cc',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/6f33a998fab91afd9ca0f344c9eddd862aedc4cc'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/b8c8335d951789d0450499e6751d582591a3cca1',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b8c8335d951789d0450499e6751d582591a3cca1',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/b8c8335d951789d0450499e6751d582591a3cca1',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b8c8335d951789d0450499e6751d582591a3cca1/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f'
      }
    ]
  },
  {
    'sha': '8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-17T22:20:49Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-17T22:20:49Z'
      },
      'message': 'Add an accelerator to the Copy button.',
      'tree': {
        'sha': '3d7fe5cb6964c8302b9dad374a8d2fd0cbf089e5',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/3d7fe5cb6964c8302b9dad374a8d2fd0cbf089e5'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/8d6e1f2e585ec189ad2aeeaef56194fff0f42f6f/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '83bea0a942de0e5bcebe38852086df816e351fd1',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/83bea0a942de0e5bcebe38852086df816e351fd1',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/83bea0a942de0e5bcebe38852086df816e351fd1'
      }
    ]
  },
  {
    'sha': '83bea0a942de0e5bcebe38852086df816e351fd1',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-17T22:05:21Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-17T22:05:21Z'
      },
      'message': ""Don't make the copy button expand the pastes\n\nCopy button no longer takes up extra vertical space unless the expand\nbutton is also present."",
      'tree': {
        'sha': '492e007b164d9c452833ba453e35455f1dc21c07',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/492e007b164d9c452833ba453e35455f1dc21c07'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/83bea0a942de0e5bcebe38852086df816e351fd1',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/83bea0a942de0e5bcebe38852086df816e351fd1',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/83bea0a942de0e5bcebe38852086df816e351fd1',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/83bea0a942de0e5bcebe38852086df816e351fd1/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '0bd6911bde473c35758989902134f2a2b3f6add3',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/0bd6911bde473c35758989902134f2a2b3f6add3',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/0bd6911bde473c35758989902134f2a2b3f6add3'
      }
    ]
  },
  {
    'sha': '0bd6911bde473c35758989902134f2a2b3f6add3',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T22:04:26Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T22:04:26Z'
      },
      'message': 'Show scrollbar in a cut-off text paste.',
      'tree': {
        'sha': '8f43a6ac968b3a2994b478f3e89113d194d21476',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/8f43a6ac968b3a2994b478f3e89113d194d21476'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/0bd6911bde473c35758989902134f2a2b3f6add3',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/0bd6911bde473c35758989902134f2a2b3f6add3',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/0bd6911bde473c35758989902134f2a2b3f6add3',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/0bd6911bde473c35758989902134f2a2b3f6add3/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': '3254d91978c79aa19229305f22c8c4c1c89a17eb',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3254d91978c79aa19229305f22c8c4c1c89a17eb',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/3254d91978c79aa19229305f22c8c4c1c89a17eb'
      }
    ]
  },
  {
    'sha': '3254d91978c79aa19229305f22c8c4c1c89a17eb',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T22:02:50Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T22:02:50Z'
      },
      'message': 'Ensure hyperlinks can be used in multiline pastes.',
      'tree': {
        'sha': '30081d08db29ce7857fd23bd0e91865dbc8e7d91',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/30081d08db29ce7857fd23bd0e91865dbc8e7d91'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/3254d91978c79aa19229305f22c8c4c1c89a17eb',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3254d91978c79aa19229305f22c8c4c1c89a17eb',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/3254d91978c79aa19229305f22c8c4c1c89a17eb',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/3254d91978c79aa19229305f22c8c4c1c89a17eb/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'b7dc812ca6c4d339bca739cc62534ead439a6eab',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b7dc812ca6c4d339bca739cc62534ead439a6eab',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/b7dc812ca6c4d339bca739cc62534ead439a6eab'
      }
    ]
  },
  {
    'sha': 'b7dc812ca6c4d339bca739cc62534ead439a6eab',
    'commit': {
      'author': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T20:54:42Z'
      },
      'committer': {
        'name': 'Arild Fines',
        'email': 'arild.fines@broadpark.no',
        'date': '2013-01-16T20:54:42Z'
      },
      'message': 'Allow larger text pastes before the expand button appears',
      'tree': {
        'sha': 'af6ab5f2a110ecf902f5babdaafc52ba378108cf',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/git/trees/af6ab5f2a110ecf902f5babdaafc52ba378108cf'
      },
      'url': 'https://api.github.com/repos/ArildF/MetroFire/git/commits/b7dc812ca6c4d339bca739cc62534ead439a6eab',
      'comment_count': 0
    },
    'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b7dc812ca6c4d339bca739cc62534ead439a6eab',
    'html_url': 'https://github.com/ArildF/MetroFire/commit/b7dc812ca6c4d339bca739cc62534ead439a6eab',
    'comments_url': 'https://api.github.com/repos/ArildF/MetroFire/commits/b7dc812ca6c4d339bca739cc62534ead439a6eab/comments',
    'author': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'committer': {
      'login': 'ArildF',
      'id': 63673,
      'avatar_url': 'https://secure.gravatar.com/avatar/d54fb864d82a85386ba1b83fa4f158e0?d=https://a248.e.akamai.net/assets.github.com%2Fimages%2Fgravatars%2Fgravatar-user-420.png',
      'gravatar_id': 'd54fb864d82a85386ba1b83fa4f158e0',
      'url': 'https://api.github.com/users/ArildF',
      'html_url': 'https://github.com/ArildF',
      'followers_url': 'https://api.github.com/users/ArildF/followers',
      'following_url': 'https://api.github.com/users/ArildF/following{/other_user}',
      'gists_url': 'https://api.github.com/users/ArildF/gists{/gist_id}',
      'starred_url': 'https://api.github.com/users/ArildF/starred{/owner}{/repo}',
      'subscriptions_url': 'https://api.github.com/users/ArildF/subscriptions',
      'organizations_url': 'https://api.github.com/users/ArildF/orgs',
      'repos_url': 'https://api.github.com/users/ArildF/repos',
      'events_url': 'https://api.github.com/users/ArildF/events{/privacy}',
      'received_events_url': 'https://api.github.com/users/ArildF/received_events',
      'type': 'User'
    },
    'parents': [
      {
        'sha': 'd8e2cdedc57f32d47a823dc1f5db6d1ebb29be20',
        'url': 'https://api.github.com/repos/ArildF/MetroFire/commits/d8e2cdedc57f32d47a823dc1f5db6d1ebb29be20',
        'html_url': 'https://github.com/ArildF/MetroFire/commit/d8e2cdedc57f32d47a823dc1f5db6d1ebb29be20'
      }
    ]
  }
]
";
	}
}
