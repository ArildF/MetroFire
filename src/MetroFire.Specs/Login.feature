Feature: Login
	In order to get access to Campfire rooms
	As a Campfire user
	I want to enter my account information

@backgroundtestscheduler
Scenario: Invalid account name
	Given that 'Fobsd' is not a valid Campfire account name
	When I enter 'Fobsd' for the account name on the login screen
	And I wait 3 seconds
	Then an account name error message should be displayed on the login screen
	And the account name should not be verified on the login screen

@backgroundtestscheduler
Scenario: Valid account name
	Given that 'Foobar' is a valid Campfire account name
	When I enter 'Foobar' for the account name on the login screen
	And I enter a token
	And I wait 3 seconds
	Then an error message should not be displayed on the login screen
	And the account name should be verified on the login screen
	And I should be able to log in


@backgroundtestscheduler
Scenario: Error message if Campfire cannot be reached
	Given that Campfire cannot be reached while verifying account name
	When I enter 'Foobar' for the account name on the login screen
	And I wait 3 seconds
	Then a connection error message should be displayed on the login screen
	And an account name error message should be displayed on the login screen
	And the account name should not be verified on the login screen
	And I should not be able to log in

@backgroundtestscheduler
Scenario: Retry if Campfire initially cannot be reached
	Given that Campfire cannot be reached while verifying account name
	And that 'Foobar' is a valid Campfire account name
	When I enter 'Foobar' for the account name on the login screen
	And I enter the token '12345' on the login screen
	And I wait 3 seconds
	And then Campfire can be reached while verifying account name
	And I click Retry on the login screen
	And I wait 3 seconds
	Then I should be able to log in


@backgroundtestscheduler
Scenario: Go to settings if network error
	Given that Campfire cannot be reached while verifying account name
	When I enter 'Foobar' for the account name on the login screen
	And I enter a token
	And I wait 3 seconds
	And I click Proxy settings on the login screen
	Then Proxy Settings should be the active module

@backgroundtestscheduler
Scenario: Invalid token
	Given that '12345' is an invalid token
	And that 'Foobar' is a valid Campfire account name
	When I enter 'Foobar' for the account name on the login screen
	And I enter the token '12345' on the login screen
	And I wait 3 seconds
	And I click Login
	And I wait 3 seconds
	Then the token should be marked as wrong on the login screen
	And it should not be logging in


@backgroundtestscheduler
Scenario: Valid token
	Given that '12345' is a valid token
	And that 'Foobar' is a valid Campfire account name
	When I enter 'Foobar' for the account name on the login screen
	And I enter the token '12345' on the login screen
	And I wait 3 seconds
	And I click Login
	And I wait 3 seconds
	Then the token should not be marked as wrong on the login screen
	And it should not be logging in



