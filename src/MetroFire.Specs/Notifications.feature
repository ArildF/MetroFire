Feature: Notifications
	In order to be notified of important messages
	As a chatter
	I want to get notifications about important messages

Background: 
	Given that I am logged in
	And a room called "Test"
	And that I have joined the room "Test"

Scenario: Notify on message
	Given that my settings are set to flash taskbar on any message
	When the message "Hello world" is received for room "Test"
	Then the taskbar should flash

Scenario: Don't notify on message from self
	Given that my settings are set to flash taskbar on any message
	When I send the message "Hello world" to room "Test"
	Then the taskbar should not flash

@backgroundtestscheduler
Scenario: Notify with toast
	Given that my settings are set to show a toast on any message
	When the message "Hello world" is received for room "Test"
	Then a toast should appear containing the words "Hello world" 
	Then after 8 seconds there should be 0 toasts

Scenario: Close toast manually
	Given that my settings are set to show a toast on any message
	And the message "Hello world" is received for room "Test"
	When I close the toast by clicking on the X
	Then there should be 0 toasts
