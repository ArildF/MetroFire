using System.IO;
using Machine.Specifications;
using Rogue.MetroFire.CampfireClient.Serialization;

namespace Rogue.MetroFire.CampfireClient.Specs
{
	public class ApiContext
	{
		private Establish context =
			() => api = CreateApi();

		private static CampfireApi CreateApi()
		{
			var lines = File.ReadAllLines("Creds.txt");
			var url = lines[0];
			var token = lines[1];

			var api = new CampfireApi();
			api.SetLoginInfo(new LoginInfo(url, token));

			return api;
		}
		protected static CampfireApi api;
	}
	[Subject(typeof(Account))]
	public class When_retrieving_account_info : ApiContext
	{

		Because of = () => account = api.GetAccountInfo();

		It should_contain_data = () => account.CreatedAt.Year.ShouldEqual(2011);

		private static Account account;
	}

	[Subject(typeof(Room))]
	public class When_retrieving_room_list : ApiContext
	{
		Because of = () => rooms = api.ListRooms();

		It should_contain_list_of_rooms = () => rooms.ShouldNotBeEmpty();

		private static Account account;
		private static Room[] rooms;
	}

	[Subject(typeof(Room))]
	public class When_retrieving_presence_list : ApiContext
	{
		Because of = () => rooms = api.ListPresence();

		It should_return_list_of_rooms = () => rooms.ShouldNotBeNull();

		private static Room[] rooms;


	}

}
