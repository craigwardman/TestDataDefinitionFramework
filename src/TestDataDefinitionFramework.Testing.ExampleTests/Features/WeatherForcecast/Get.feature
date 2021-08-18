Feature: Get Weather Forcecast
	Gets the weather forecast
	Based on the selection of the available summaries
	In order to show an example of using a repository

Scenario: When no summaries exist, the no forecast items are returned
	Given the summaries repository returns no items
	When a request is made to get the weather forecast
	Then the weather forecast contains '0' items

Scenario: When summaries exist, the forecast is returned as expected
	Given the summaries repository returns 'Sunny,Cloudy,Breezy'
	When a request is made to get the weather forecast
	Then the weather forecast contains '3' items