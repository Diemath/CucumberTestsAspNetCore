Feature: Old Tickets Deleting
	In order to ensure that all options go from configurations. 
	In order to have an ability to change options in the most efficient way.
	In order to ensure that not all tickets will be deleted.

Scenario: Delete old tickets
	Given today is "6/14/2019"
	And configuration has value for days limit before deleting of tickets - "10"
	And three tickets exist. First ticket was created at "6/14/2019"
	And second ticket was created at "6/3/2019"
	And third ticket was created at "6/2/2019"
	When old tickets deleting is called
	Then first ticket created at "6/14/2019" wont be deleted
	Then second ticket created at "6/3/2019" will be deleted
	Then third ticket created at "6/2/2019" will be deleted

