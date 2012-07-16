Feature: Login
	In order to get access to Campfire rooms
	As a Campfire user
	I want to enter my account information

@backgroundtestscheduler
Scenario: Invalid account name
	Given that 'Fobsd' is not a valid Campfire account name
	When I enter 'Fobsd' for the account name on the login screen
	And I wait 3 seconds
	Then an error message should be displayed on the login screen
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
	And an error message should be displayed on the login screen
	And the account name should not be verified on the login screen
	And I should not be able to log in