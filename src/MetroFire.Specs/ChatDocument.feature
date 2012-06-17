﻿Feature: Chat document
	In order to view and respond to Campfire messages
	As a Campfire chatter
	I want to have chat messages rendered and formatted in a pleasing way

Background: 
	Given a user 'Testuser'
	
Scenario: Display simple text message
	When I add the message "Hello world" from user 'Testuser'
	Then the message should be displayed like "<Testuser> Hello world"

Scenario: Emotes
	When I add the message "/me is emotional" from user 'Testuser'
	Then the message should be displayed like "Testuser is emotional"
	And the message should be displayed in italics

Scenario: Room joins
	When user 'Testuser' joins the room
	Then the message should be displayed like "Testuser entered the room"
	And the message should be displayed in italics

Scenario: Room leaves
	When user 'Testuser' leaves the room
	Then the message should be displayed like "Testuser left the room"
	And the message should be displayed in italics

Scenario: Room kicks
	When user 'Testuser' is kicked from the room
	Then the message should be displayed like "Testuser was kicked from the room"
	And the message should be displayed in italics

Scenario: Hyperlinks
	When I add the message "This is a hyperlink: http://www.test.com" from user 'Testuser'
	Then the message should be displayed like "<Testuser> This is a hyperlink: http://www.test.com"
	And "http://www.test.com" should be a hyperlink
