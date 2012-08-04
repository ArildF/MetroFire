Feature: Notification settings
	In order to get notified of important events
	As a Campfire user
	I want to edit notifications

Background: 
	Given that I am logged in

Scenario: Display notification settings
	Given a new notification
	And the notification has the following triggers:
	| TriggerType   | MatchText | MatchUser | MatchRoom |
	| Room Activity | Boink     |           |           |
	| User Enters   |           |Arild      |           |
	| User message  |           |           |Oslo       |
	| User paste    |           |           |           |

	And the notification has the following actions
	| Action type | Value |
	| Show toast  |       |
	| Flash taskbar|       |
	| Flash taskbar| |

	Then the screen should read like this:
	| Line                                            |
	| When                                            |
	| there is any activity matching the text 'Boink' |
	| a user enters matching the username 'Arild'    |
	| a user posts a message in a room matching 'Oslo'|
	| a user posts a file or picture                |
	| then                                            |
	| show a toast                                    |
	| flash the task bar                              |
	| flash the task bar                               |

Scenario: Delete notification
	Given a new notification
	When I close notification #1
	Then there should be 0 notifications

Scenario: Edit trigger
	Given a new notification
	And the notification has the following triggers:
	| Type          | MatchText | MatchUser | MatchRoom |
	| Room activity | Boink     |           |           |
	When I double click on trigger #1
	Then trigger #1 should be editable
	And all triggers except #1 should not be editable
	And all actions should not be editable

Scenario: Editing one trigger should stop editing another
	Given a new notification
	And the notification has the following triggers:
	| Type          | MatchText | MatchUser | MatchRoom |
	| Room activity | Boink     |           |           |
	| Room activity | Moink     |           |           |
	When I double click on trigger #1
	And I double click on trigger #2
	Then trigger #2 should be editable
	And all triggers except #2 should not be editable
	And all actions should not be editable

Scenario: Adding new notification should add a trigger and action in edit mode
	When I add a new notification
	Then the new notification should have a trigger
	And trigger #1 should be editable
	And the new notification should have an action
	And action #1 should be editable

Scenario: Adding new trigger should select it and set it directly into edit mode
	Given a new notification
	When I add a new trigger
	Then the new trigger should be editable
	Then the new trigger should be selected
	
Scenario: Adding new action should select it and set it directly into edit mode
	Given a new notification
	When I add a new action
	Then the new action should be editable
	And the new action should be selected

