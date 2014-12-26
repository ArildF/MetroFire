Feature: Chat document
	In order to view and respond to Campfire messages
	As a Campfire chatter
	I want to have chat messages rendered and formatted in a pleasing way

Background: 
	Given that I am logged in
	And a user 'Testuser'
	And a room called "Test"
	And that I have joined the room "Test"
	And that the current room is "Test"
	
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

Scenario Outline: Hyperlinks
	When I add the message "This is a hyperlink: <hyperlink> This is text following a hyperlink" from user 'Testuser'
	Then the message should be displayed like "<Testuser> This is a hyperlink: <hyperlink> This is text following a hyperlink"
	And "<hyperlink>" should be a hyperlink
	Examples:
	| hyperlink                                                   |
	| http://www.test.com                                         |
	| http://www.vg.no                                            |
	| https://www.ikke.no                                         |
	| http://intranett.local                                      |
	| http://nb.wikipedia.org/Jordstråler                         |
	| http://no.wikipedia.org/wiki/Portal:Forside                 |
	| http://en.wikipedia.org/wiki/Decembrist_revolt#cite_note-11 |



Scenario: Image pastes without memory leaks
	When I add 40 image pastes to the room
	And I cancel all the image pastes in the room
	Then there should be 0 PasteViewModels in the system

Scenario Outline: Post youtube links
	When I add the message "<Message>" from user 'Testuser'
	Then the message should be displayed as an inline youtube video
	Examples:
	|Message|
	|http://www.youtube.com/watch?feature=player_embedded&v=o-50GjySwew#!|
	|http://www.youtube.com/watch?feature=player_embedded&v=o-50GjySwew|

Scenario Outline: Emoticons
	When I add the message "<Message>" from user 'Testuser'
	Then the message '<Message>' should have the '<Emoticon>' replaced by a graphic
	Examples:
	| Message         | Emoticon |
	| That's good :-) | :-)      |
	| That's good :)  | :)       |
	| Yay! :D         | :D       |
	| Yay! :-D        | :-D      |
	| Oh no... :-(    | :-(      |
	| Oh no... :(     | :(       |
	| Hmm... :-S      | :-S      |
	| Hmm... :S       | :S       |


Scenario Outline: Custom highlighting
	Given that there is a setting to render messages with the word '<Triggerword>' in '<Color>'
	When I add the message "<Message>" from user 'Testuser'
	Then the message '<ExpectedMessage>' should be displayed rendered in '<Color>'
	Examples: 
	| Triggerword | Color | Message             | ExpectedMessage                |
	| arkbuilder  | Red   | Jeg tar arkbuilder! | <Testuser> Jeg tar arkbuilder! |





