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

    public class WeatherStation : IWeatherStation
    {
        private readonly List<IWeatherDisplay> _observers;
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public WeatherStation()
        {
            _observers = new List<IWeatherDisplay>();
        }

        public float Temperature => _temperature;
        public float Humidity => _humidity;
        public float Pressure => _pressure;
        public int DisplayCount => _observers.Count;

        public void RegisterDisplay(IWeatherDisplay display)
        {
            if (display != null && !_observers.Contains(display))
            {
                _observers.Add(display);
            }
        }

        public void RemoveDisplay(IWeatherDisplay display)
        {
            _observers.Remove(display);
        }

        public void SetWeatherData(float temperature, float humidity, float pressure)
        {
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;
            NotifyObservers();
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                if (observer is IWeatherObserver weatherObserver)
                {
                    weatherObserver.Update(_temperature, _humidity, _pressure);
                }
            }
        }
    }

    public interface IWeatherObserver
    {
        void Update(float temperature, float humidity, float pressure);
    }

    public class MobileAppDisplay : IWeatherDisplay, IWeatherObserver
    {
        public float Temperature { get; private set; }
        public float Humidity { get; private set; }
        public float Pressure { get; private set; }
        public string Name => "Mobile App";

        public void Update(float temperature, float humidity, float pressure)
        {
            Temperature = temperature;
            Humidity = humidity;
            Pressure = pressure;
        }

        public override string ToString()
        {
            return $"Mobile App - Temp: {Temperature:F1}°F, Humidity: {Humidity:F1}%, Pressure: {Pressure:F1} inHg";
        }
    }

    public class WebsiteDisplay : IWeatherDisplay, IWeatherObserver
    {
        public float Temperature { get; private set; }
        public float Humidity { get; private set; }
        public float Pressure { get; private set; }
        public string Name => "Website";

        public void Update(float temperature, float humidity, float pressure)
        {
            Temperature = temperature;
            Humidity = humidity;
            Pressure = pressure;
        }

        public override string ToString()
        {
            return $"Website - Temp: {Temperature:F1}°F, Humidity: {Humidity:F1}%, Pressure: {Pressure:F1} inHg";
        }
    }

    public class DigitalBillboardDisplay : IWeatherDisplay, IWeatherObserver
    {
        public float Temperature { get; private set; }
        public float Humidity { get; private set; }
        public float Pressure { get; private set; }
        public string Name => "Digital Billboard";

        public void Update(float temperature, float humidity, float pressure)
        {
            Temperature = temperature;
            Humidity = humidity;
            Pressure = pressure;
        }

        public override string ToString()
        {
            return $"Billboard - Temp: {Temperature:F1}°F, Humidity: {Humidity:F1}%, Pressure: {Pressure:F1} inHg";
        }
    }

    /// <summary>
    /// Factory for creating the observer pattern with weather station and display coordination.
    /// </summary>
    public static class ObserverPattern
    {
        public static IWeatherStation Create()
        {
            return new WeatherStation();
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
