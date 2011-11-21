Feature: Room management
	In order to leave and join rooms
	As a CampFire user
	I want to be able to leave and join rooms

Background: 
	Given that I am logged in
	And a room called "Test"
	And that I have joined the room "Test"

Scenario: Leave room
	When I click the leave room button in room "Test"
	Then I should leave room "Test"
	And the lobby should be active
	And streaming should be disconnected for room "Test"
