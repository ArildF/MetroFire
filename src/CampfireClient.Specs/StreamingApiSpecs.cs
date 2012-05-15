using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading;
using Machine.Specifications;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient.Specs
{
	public class When_streaming_messages_from_Campfire : ApiContext
	{
		Establish context = () =>
			{
				_id = FindJoinedRoom();
				var subject = new Subject<ConnectionState>();
				subject.Subscribe(cs => _connectionStates.Add(cs));
				_disposable = api.Stream(_id, msg =>
					{
						_messages.Add(msg);
						if (msg.Type == MessageType.TextMessage)
						{
							_event.Set();
						}
					}, subject);

				_msg = @"Hello
world " + Guid.NewGuid();
			};

		private Because of = () =>
			{
				Thread.Sleep(16000);
				Debug.WriteLine("Speaking");
				api.Speak(_id, _msg);
				_event.WaitOne(8000);
			};

		It should_have_been_streamed = () => _messages.ShouldContain(msg => msg.Body == _msg);

		It should_have_proper_created_date = () => _messages.ShouldEachConformTo(msg => msg.CreatedAt != DateTime.MinValue);
		It should_have_room_id = () => _messages.ShouldEachConformTo(msg => msg.RoomId > 0);

		It should_have_reported_streaming_connected = () => _connectionStates.ShouldContain(cs => cs.Connected);

		private static readonly AutoResetEvent _event = new AutoResetEvent(false);

		private static readonly List<Message> _messages = new List<Message>();
		private static int _id;
		private static IDisposable _disposable;
		private static string _msg;
		private static readonly List<ConnectionState> _connectionStates = new List<ConnectionState>();
	}

	public class When_streaming_tweet_messages_from_Campfire : ApiContext
	{
		Establish context = () =>
			{
				_id = FindJoinedRoom();
				_disposable = api.Stream(_id, msg =>
					{
						if (msg.Type == MessageType.TweetMessage)
						{
							_tweetMessage = msg;
							_event.Set();
						}
					}, new Subject<ConnectionState>());
			};

		Because of = () =>
			{
				Thread.Sleep(8000);
				api.Speak(_id, "https://twitter.com/#!/GarethWild/status/201975545645379584");
				_event.WaitOne(8000);
			};

		It should_be_posted_as_a_tweet_message = () => _tweetMessage.Tweet.ShouldNotBeNull();

		It should_post_that_particular_tweet_message = () => _tweetMessage.Tweet.Id.ShouldEqual("201975545645379584");

		It should_post_it_as_a_tweet_from_gareth_wild = () => _tweetMessage.Tweet.AuthorUsername.ShouldEqual("GarethWild");
		private static int _id;
		private static Message _tweetMessage;
		private static IDisposable _disposable;
		private static AutoResetEvent _event = new AutoResetEvent(false);
	}

	public class When_cancelling_streaming_from_Campfire : ApiContext
	{
		Establish context = () =>
			{
				_id = FindJoinedRoom();
				_disposable = api.Stream(_id, msg => _messages.Add(msg), new Subject<ConnectionState>());

				api.Speak(_id, "Hello");

				Thread.Sleep(TimeSpan.FromSeconds(8));
			};

		Because of = () =>
			{
				_disposable.Dispose();
				api.Speak(_id, "World");
			};

		It should_contain_hello = () => _messages.ShouldContain(msg => msg.Body == "Hello");
		It should_not_contain_world = () => _messages.ShouldNotContain(msg => msg.Body == "World");

		private static readonly List<Message> _messages = new List<Message>();
		private static IDisposable _disposable;
		private static int _id;
	}
}
