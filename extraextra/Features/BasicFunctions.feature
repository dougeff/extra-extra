Feature: basicFunctions
	Basic functions of the app

@basic
Scenario: Get Query Results
	Given I have entered text into the query field
	When I press search
	Then it should fetch query results
	And they should be minimized with a button to expand

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

Scenario: Results should be empty when program starts
	Given I double-clicked the icon
	When the program starts
	Then the results should be empty

Scenario: Results should be clickable
	Given I have a query result
	When I click one of the results
	Then it should take me to a website

Scenario: Result should disappear if deleted
	Given I have a query result
	When I click the delete button
	Then the query result should disappear
	And never be fetched again

Scenario: Result should be maximized when maximize button is clicked
	Given I have a query result
	And it is minimized
	When I click the maximize button
	Then it should be maximized

Scenario: Result should be minimized when minimize button is clicked
	Given I have a query result
	And it is maximized
	When I click the minimize button
	Then it should be minimized

Scenario: Fetching should cease when time interval is being changed
	Given I have set the time interval
	When I change the time interval
	Then the countdown should freeze and nothing should be fetched