/*
Observer Pattern

Summary:
The Observer Pattern defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically. It promotes loose coupling between the subject and its observers.

Problem to Solve:
In a weather monitoring system, multiple display devices (mobile app, website, digital billboard) need to show current weather data. When the weather station updates its readings, all displays should automatically refresh with the new data. The challenge is to notify multiple displays without tightly coupling the weather station to specific display implementations.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Behavioral
{
    /// <summary>
    /// Observer interface representing weather display components that receive updates from the weather station.
    /// </summary>
    public interface IWeatherDisplay
    {
        float Temperature { get; }
        float Humidity { get; }
        float Pressure { get; }
        string Name { get; }
    }

    /// <summary>
    /// Subject interface for the weather station that maintains and notifies a list of observer displays.
    /// </summary>
    public interface IWeatherStation
    {
        void RegisterDisplay(IWeatherDisplay display);
        void RemoveDisplay(IWeatherDisplay display);
        void SetWeatherData(float temperature, float humidity, float pressure);
        float Temperature { get; }
        float Humidity { get; }
        float Pressure { get; }
        int DisplayCount { get; }
    }

    /// <summary>
    /// Factory for creating the observer pattern with weather station and display coordination.
    /// </summary>
    public static class ObserverPattern
    {
        public static IWeatherStation Create()
        {
            throw new NotImplementedException("Implement the weather station with observer pattern");
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. WeatherStation class implementing IWeatherStation:
       - Maintain a list of registered weather displays
       - RegisterDisplay/RemoveDisplay manage the observer list
       - SetWeatherData updates internal state and notifies all observers
       - Track DisplayCount for the number of registered displays

    2. Display classes implementing IWeatherDisplay:
       - MobileAppDisplay: Updates when notified by weather station
       - WebsiteDisplay: Updates when notified by weather station  
       - DigitalBillboardDisplay: Updates when notified by weather station
       - Each display should store the weather data when updated

    3. Observer notification mechanism:
       - Weather station calls an update method on each registered display
       - Displays update their internal state with new weather data
       - Updates happen automatically when SetWeatherData is called

    4. Update Create method to:
       - Return a weather station instance that implements the observer pattern
       - Weather station should be ready to register displays and notify them

    The tests will verify that:
    - Displays can be registered and removed from the weather station
    - SetWeatherData notifies all registered displays
    - Displays receive and store the correct weather data
    - Observer list management works correctly
    */
}
