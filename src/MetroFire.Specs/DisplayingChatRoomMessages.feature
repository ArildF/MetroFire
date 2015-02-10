Feature: Receiving chat room messages
	In order to see messages from others
	As a CampFire user
	I want to see the messages from others

Background: 
	Given that I am logged in
	And a room called "Test"
	And a user 'Testuser'
	And that I am user 'Testuser'
	And that I have joined the room "Test"


Scenario: Receive a single message
	When the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message     |
	| 1  | Hello world |
	Then the following messages should be displayed in room "Test" in order:
	| Id | Message |
	| 1  | <Testuser> Hello world    |

Scenario: Receive multiple messages
	When the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message |
	| 1  | Ohai    |
	| 2  | Hai     |
	| 3  | Test    |
	Then the following messages should be displayed in room "Test" in order:
	| Id | Message |
	| 1  | <Testuser> Ohai    |
	| 2  | <Testuser> Hai     |
	| 3  | <Testuser> Test    |

Scenario: Receive multiple messages multiple times
	When the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message |
	| 1  | Ohai    |
	| 2  | Hai     |
	| 3  | Test    |
	And the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message |
	| 4  | Blah    |
	| 5  | Nah     |
	| 6  | Zah    |
	Then the following messages should be displayed in room "Test" in order:
	| Message |
	| <Testuser> Ohai    |
	| <Testuser> Hai     |
	| <Testuser> Test    |
	| <Testuser> Blah    |
	| <Testuser> Nah     |
	| <Testuser> Zah    |

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
	When the streaming is disconnected for room "Test"
	And the streaming is reconnected for room "Test"
	And we wait 20 seconds
	Then older messages should have been requested for room "Test"

Scenario: Show old messages sent while disconnected
	Given that the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message |
	| 1  | One     |
	| 2  | Two     |
	| 3  | Three   |
	When the streaming is disconnected for room "Test"
	And the streaming is reconnected for room "Test"
	And the following messages are received from user 'Testuser' for room "Test" in order:
	| Id | Message |
	| 6  | Six     |
	| 7  | Seven   |
	And the following messages are received from user 'Testuser' for room "Test" in order:
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
	| 1  | <Testuser> One     |
	| 2  | <Testuser> Two     |
	| 3  | <Testuser> Three   |
	| 4  | <Testuser> Four    |
	| 5  | <Testuser> Five    |
	| 6  | <Testuser> Six     |
	| 7  | <Testuser> Seven   |

Scenario: Change topic
	When the topic is changed to "To pic" for room "Test"
	Then the topic should be "To pic" for room "Test"

Scenario Outline: Meta messages
	When the room "Test" receives a <MessageType> message
	Then the message "<MessageBody>" should be displayed in room "Test"
	Examples:
	| MessageType               | MessageBody                        |
	| LockMessage               | Testuser locked the room           |
	| UnlockMessage             | Testuser unlocked the room         |
	| AllowGuestsMessage        | Testuser turned on guest access    |
	| DisallowGuestsMessage     | Testuser turned off guest access   |
	| ConferenceCreatedMessage  | Testuser started a conference call |
	| ConferenceFinishedMessage | Testuser ended a conference call   |
	| IdleMessage               | Testuser is idle                 |
	| UnidleMessage             | Testuser is back                   |
	| SoundMessage              | Testuser played a sound            |
		
