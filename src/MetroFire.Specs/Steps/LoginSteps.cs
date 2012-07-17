using System;
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

		[Given(@"that Campfire cannot be reached while verifying account name")]
		public void GivenThatCampfireCannotBeReachedWhileVerifyingAccountName()
		{
			_campfireApiFake.ThrowOnValidatingAccount();
		}

		[When(@"then Campfire can be reached while verifying account name")]
		public void WhenThenCampfireCanBeReachedWhileVerifyingAccountName()
		{
			_campfireApiFake.DontThrowOnValidatingAccount();
		}


		[When(@"I enter '(.*)' for the account name on the login screen")]
		public void WhenIEnterFobsdForTheAccountNameOnTheLoginScreen(string accountName)
		{
			_context.LoginViewModel.Account = accountName;
		}

		[When(@"I enter a token")]
		public void WhenIEnterAToken()
		{
			_context.LoginViewModel.Token = "1235lkdjfglkdf";
		}


		[When(@"I click Retry on the login screen")]
		public void WhenIClickRetryOnTheLoginScreen()
		{
			_context.LoginViewModel.RetryCommand.Execute(null);
		}

		[When(@"I click Proxy settings on the login screen")]
		public void WhenIClickProxySettingsOnTheLoginScreen()
		{
			_context.LoginViewModel.ProxySettingsCommand.Execute(null);
		}

		[Given(@"that '(.*)' is an invalid token")]
		public void GivenThat12345IsAnInvalidToken(string token)
		{
			_campfireApiFake.CorrectToken = Guid.NewGuid().ToString();
		}

		[Given(@"that '(.*)' is a valid token")]
		public void GivenThat12345IsAvalidToken(string token)
		{
			_campfireApiFake.CorrectToken = token;
		}


		[When(@"I enter the token '(.*)' on the login screen")]
		public void WhenIEnterTheToken12345OnTheLoginScreen(string token)
		{
			_context.LoginViewModel.Token = token;
		}


		[When(@"I click Login")]
		public void WhenIClickLogin()
		{
			_context.LoginViewModel.LoginCommand.Execute(null);
		}




		[Then(@"an account name error message should be displayed on the login screen")]
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

		[Then(@"a progress animation should be displayed next to the account name text box")]
		public void ThenAProgressAnimationShouldBeDisplayedNextToTheAccountNameTextBox()
		{
			_context.LoginViewModel.IsVerifyingAccountInProgress.Should().BeTrue();
		}

		[Then(@"a connection error message should be displayed on the login screen")]
		public void ThenAConnectionErrorMessageShouldBeDisplayedOnTheLoginScreen()
		{
			_context.LoginViewModel.ShowConnectionError.Should().BeTrue();
			_context.LoginViewModel.ConnectionErrorMessage.Should().NotBeNullOrEmpty();
		}

		[Then(@"I should not be able to log in")]
		public void ThenIShouldNotBeAbleToLogIn()
		{
			_context.LoginViewModel.LoginCommand.CanExecute(null).Should().BeFalse();
		}

		[Then(@"I should be able to log in")]
		public void ThenIShouldBeAbleToLogIn()
		{
			_context.LoginViewModel.LoginCommand.CanExecute(null).Should().BeTrue();
		}

		[Then(@"Proxy Settings should be the active module")]
		public void ThenProxySettingsShouldBeTheActiveModule()
		{
			
		}

		[Then(@"the token should be marked as wrong on the login screen")]
		public void ThenTheTokenShouldBeMarkedAsWrongOnTheLoginScreen()
		{
			_context.LoginViewModel.IsTokenInError.Should().BeTrue();
		}

		[Then(@"the token should not be marked as wrong on the login screen")]
		public void ThenTheTokenShouldNotBeMarkedAsWrongOnTheLoginScreen()
		{
			_context.LoginViewModel.IsTokenInError.Should().BeFalse();
		}


		[Then(@"it should not be logging in")]
		public void ThenItShouldNotBeLoggingIn()
		{
			_context.LoginViewModel.IsLoggingIn.Should().BeFalse();
		}







	}
}
