Feature: basicFunctions
	Basic functions of the app

@basic
Scenario: Get Query Results
	Given I have entered text into the query field
	When I press search
	Then it should fetch query results
	And the results number should be displayed

Scenario: Query results should not duplicate
	Given I have a query result
	When I press search
	Then it should not redisplay the results

Scenario: Query items should not duplicate in query results
	Given I have a query result
	When I press search
	Then it should not redisplay the same items

Scenario: Query should not fetch blank
	Given I have left the text field blank
	When I press search
	Then it should not fetch query results

Scenario: Query should not fetch default text
	Given I have not entered text into the query field
	When I press search
	Then it should not fetch query results

Scenario: Refresh Query Results
	Given I have a query result
	And I have set the time interval
	When the interval ends
	Then it should fetch more query results
	And append new results to the old results

Scenario: Update Icon with new Results Total
	Given The application is minimized
	And I have a query result
	And I have set the time interval
	When the interval ends
	Then it should update the icon with a total of new results found

Scenario: Results should be clickable
	Given I have a query result
	When I click one of the results
	Then it should take me to a website

Scenario: Result should disappear if deleted
	Given I have a query result
	When I click the delete button
	Then the query result should disappear
	And never be fetched again

Scenario: Fetching should cease when time interval is being changed
	Given I have set the time interval
	When I change the time interval
	Then the countdown should freeze and nothing should be fetched