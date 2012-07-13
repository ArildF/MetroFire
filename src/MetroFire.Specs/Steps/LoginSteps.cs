using FluentAssertions;
using TechTalk.SpecFlow;

namespace MetroFire.Specs.Steps
{
	[Binding]
	public class LoginSteps
	{
		private readonly RoomContext _context;
		private readonly CampfireApiFake _campfireApiFake;

		public LoginSteps(RoomContext context, CampfireApiFake campfireApiFake)
		{
			_context = context;
			_campfireApiFake = campfireApiFake;
		}


		[Given(@"that '(.*)' is not a valid Campfire account name")]
		public void GivenThatFobsdIsNotAValidCampfireAccountName(string accountName)
		{
			_campfireApiFake.IsInvalidAccount(accountName);
		}

		[Given(@"that '(.*)' is a valid Campfire account name")]
		public void GivenThatFoobarIsAValidCampfireAccountName(string accountName)
		{
			_campfireApiFake.IsValidAccount(accountName);
		}


		[When(@"I enter '(.*)' for the account name on the login screen")]
		public void WhenIEnterFobsdForTheAccountNameOnTheLoginScreen(string accountName)
		{
			_context.LoginViewModel.Account = accountName;
		}

		[Then(@"an error message should be displayed on the login screen")]
		public void ThenAnErrorMessageShouldBeDisplayedOnTheLoginScreen()
		{
			_context.LoginViewModel.IsAccountNameInError.Should().BeTrue();
		}

		[Then(@"an error message should not be displayed on the login screen")]
		public void ThenAnErrorMessageShouldNotBeDisplayedOnTheLoginScreen()
		{
			_context.LoginViewModel.IsAccountNameInError.Should().BeFalse();
		}


		[Then(@"the account name should not be verified on the login screen")]
		public void ThenTheAccountNameShouldNotBeVerifiedOnTheLoginScreen()
		{
			_context.LoginViewModel.IsAccountNameVerified.Should().BeFalse();
		}

		[Then(@"the account name should be verified on the login screen")]
		public void ThenTheAccountNameShouldBeVerifiedOnTheLoginScreen()
		{
			_context.LoginViewModel.IsAccountNameVerified.Should().BeTrue();
		}



	}
}
