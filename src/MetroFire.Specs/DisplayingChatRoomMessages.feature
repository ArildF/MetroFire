Feature: Receiving chat room messages
	In order to see messages from others
	As a CampFire user
	I want to see the messages from others

Background: 
	Given a room called "Test"


Scenario: Receive a single message
	When the message "Hello world" is received for room "Test"
	Then the message "Hello world" should be displayed in room "Test"

Scenario: Receive multiple messages
	When the following messages are received for room "Test" in order:
	| Message |
	| Ohai    |
	| Hai     |
	| Test    |
	Then the following messages should be displayed in room "Test" in order:
	| Message |
	| Ohai    |
	| Hai     |
	| Test    |
	
