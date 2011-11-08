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
	| Id | Message |
	| 1  | Ohai    |
	| 2  | Hai     |
	| 3  | Test    |
	Then the following messages should be displayed in room "Test" in order:
	| Id | Message |
	| 1  | Ohai    |
	| 2  | Hai     |
	| 3  | Test    |

Scenario: Receive multiple messages multiple times
	When the following messages are received for room "Test" in order:
	| Id | Message |
	| 1  | Ohai    |
	| 2  | Hai     |
	| 3  | Test    |
	And the following messages are received for room "Test" in order:
	| Id | Message |
	| 4  | Blah    |
	| 5  | Nah     |
	| 6  | Zah    |
	Then the following messages should be displayed in room "Test" in order:
	| Message |
	| Ohai    |
	| Hai     |
	| Test    |
	| Blah    |
	| Nah     |
	| Zah    |

@backgroundtestscheduler
Scenario: Show disconnections
	When the streaming is disconnected for room "Test"
	Then room "Test" should show that it is disconnected
	
@backgroundtestscheduler
Scenario: Show reconnections
	When the streaming is disconnected for room "Test"
	And the streaming is reconnected for room "Test"
	Then room "Test" should show that it is connected

@backgroundtestscheduler
Scenario: Retrieve old messages when disconnected
	Given that streaming has started for room "Test"
	When the streaming is disconnected for room "Test"
	And the streaming is reconnected for room "Test"
	And we wait 20 seconds
	Then older messages should have been requested for room "Test"

Scenario: Show old messages sent while disconnected
	Given that the following messages are received for room "Test" in order:
	| Id | Message |
	| 1  | One     |
	| 2  | Two     |
	| 3  | Three   |
	When the streaming is disconnected for room "Test"
	And the streaming is reconnected for room "Test"
	And the following messages are received for room "Test" in order:
	| Id | Message |
	| 6  | Six     |
	| 7  | Seven   |
	And the following messages are received for room "Test" in order:
	| Id | Message |
	| 1  | One     |
	| 2  | Two     |
	| 3  | Three   |
	| 4  | Four    |
	| 5  | Five    |
	| 6  | Six     |
	| 7  | Seven   |
	Then the following messages should be displayed in room "Test" in order:
	| Id | Message |
	| 1  | One     |
	| 2  | Two     |
	| 3  | Three   |
	| 4  | Four    |
	| 5  | Five    |
	| 6  | Six     |
	| 7  | Seven   |
		
