Feature: Notification settings
	In order to get notified of important events
	As a Campfire user
	I want to edit notifications

Background: 
	Given that I am logged in

Scenario: Display notification settings
	Given a new notification
	And the notification has the following triggers:
	| Type          | MatchText | MatchUser | MatchRoom |
	| Room activity | Boink     |           |           |
	And the notification has the following actions
	| Action type | Value |
	| Show toast  |       |

	Then the screen should read like this:
	| Line                                            |
	| When                                            |
	| there is any activity matching the text 'Boink' |
	| then                                            |
	| show a toast                                    |

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

	

