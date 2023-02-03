Feature: Get Weather Forecast
Gets the weather forecast
Based on the selection of the available summaries
In order to show an example of using a repository

    Scenario: When no summaries exist, the no forecast items are returned
        Given the summaries repository returns no items
        When a request is made to get the weather forecast
        Then the weather forecast contains '0' items

    Scenario: When summaries exist, the forecast is returned as expected
        Given the summaries repository returns 'Sunny,Cloudy,Breezy'
        And the summary description repository returns
          | Name   | Description                   |
          | Sunny  | Feels warm, maybe wear shorts |
          | Cloudy | Overcast dreariness           |
          | Breezy | Just a bit of wind            |
        And the summary temperature repository contains
          | Name   | High | Low |
          | Sunny  | 18   | 10  |
          | Cloudy | 11   | 5   |
          | Breezy | 8    | 2   |
        When a request is made to get the weather forecast
        Then the weather forecast contains '3' items
        And the weather forecast items match the repository data