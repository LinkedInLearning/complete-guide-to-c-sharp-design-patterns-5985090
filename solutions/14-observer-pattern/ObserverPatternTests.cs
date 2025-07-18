using NUnit.Framework;
using System;

using DotNetDesignPatterns.Patterns.Behavioral;

namespace DotNetDesignPatterns.Tests.Patterns.Behavioral
{
    /// <summary>
    /// Tests for the Observer Pattern implementation.
    /// 
    /// The Observer pattern defines a one-to-many dependency between objects so that when one
    /// object changes state, all its dependents are notified and updated automatically.
    /// It promotes loose coupling between the subject (observable) and observers.
    /// 
    /// Key components to implement:
    /// 1. Subject/Observable interface (IWeatherStation) with register/remove/notify methods
    /// 2. Observer interface (IWeatherDisplay) with update method
    /// 3. Concrete subject (WeatherStation) that maintains observer list and notifies them
    /// 4. Concrete observers (MobileAppDisplay, WebsiteDisplay, DigitalBillboardDisplay)
    /// 5. Factory method to create the weather station system
    /// 
    /// These tests verify that the Observer pattern implementation works correctly.
    /// Tests use the factory method to ensure proper component wiring.
    /// </summary>
    [TestFixture]
    public class ObserverPatternTests
    {
        /// <summary>
        /// The subject/observable that weather displays will observe.
        /// This needs to be implemented as a concrete class that maintains a list of observers.
        /// </summary>
        private IWeatherStation _weatherStation = null!;
        
        /// <summary>
        /// Observer instances representing different types of weather displays.
        /// These need to be implemented as concrete classes that implement IWeatherDisplay.
        /// </summary>
        private IWeatherDisplay _mobileApp = null!;
        private IWeatherDisplay _website = null!;
        private IWeatherDisplay _billboard = null!;

        /// <summary>
        /// Sets up test instances for each test.
        /// 
        /// Required implementation:
        /// - ObserverPattern.Create() factory method that returns IWeatherStation
        /// - Concrete display classes: MobileAppDisplay, WebsiteDisplay, DigitalBillboardDisplay
        /// - Each display class should implement IWeatherDisplay interface
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Use the factory method to create the weather station
            _weatherStation = ObserverPattern.Create();
            
            // Create test displays - must implement these classes
            _mobileApp = CreateDisplayInstance("MobileAppDisplay");
            _website = CreateDisplayInstance("WebsiteDisplay"); 
            _billboard = CreateDisplayInstance("DigitalBillboardDisplay");
        }

        /// <summary>
        /// Helper method to create display instances using reflection.
        /// This allows tests to dynamically create the concrete observer classes that need to be implemented.
        /// </summary>
        private IWeatherDisplay CreateDisplayInstance(string className)
        {
            var fullTypeName = $"DotNetDesignPatterns.Patterns.Behavioral.{className}";
            var type = Type.GetType(fullTypeName);
            if (type == null)
            {
                throw new NotImplementedException($"Must implement {className} class that implements IWeatherDisplay");
            }
            return (IWeatherDisplay)Activator.CreateInstance(type)!;
        }

        /// <summary>
        /// Tests that registering a display adds it to the observer list.
        /// 
        /// This verifies the core Observer pattern registration mechanism:
        /// - Subject maintains a list of observers
        /// - RegisterDisplay adds observers to the notification list
        /// - DisplayCount property tracks the number of registered observers
        /// 
        /// Required implementation:
        /// - List or collection to store registered observers
        /// - RegisterDisplay method that adds observers to the list
        /// - DisplayCount property that returns the number of observers
        /// </summary>
        [Test]
        public void RegisterDisplay_ShouldAddDisplayToList()
        {
            // Act
            _weatherStation.RegisterDisplay(_mobileApp);

            // Assert
            Assert.That(_weatherStation.DisplayCount, Is.EqualTo(1),
                "Weather station should have one registered display");
        }

        /// <summary>
        /// Tests that registering the same display twice doesn't create duplicates.
        /// 
        /// This verifies:
        /// - Observer list prevents duplicate registrations
        /// - Robust handling of repeated registration attempts
        /// 
        /// Required implementation:
        /// - Duplicate checking logic in RegisterDisplay method
        /// - Prevention of multiple registrations of the same observer
        /// </summary>
        [Test]
        public void RegisterDisplay_WithSameDisplayTwice_ShouldNotDuplicate()
        {
            // Act
            _weatherStation.RegisterDisplay(_mobileApp);
            _weatherStation.RegisterDisplay(_mobileApp);

            // Assert
            Assert.That(_weatherStation.DisplayCount, Is.EqualTo(1),
                "Weather station should not register the same display twice");
        }

        /// <summary>
        /// Tests that removing a display removes it from the observer list.
        /// 
        /// This verifies the Observer pattern's ability to dynamically remove observers:
        /// - RemoveDisplay method removes observers from the notification list
        /// - DisplayCount updates correctly after removal
        /// - Remaining observers are not affected
        /// 
        /// Required implementation:
        /// - RemoveDisplay method that removes observers from the list
        /// - Proper list management to maintain remaining observers
        /// </summary>
        [Test]
        public void RemoveDisplay_ShouldRemoveDisplayFromList()
        {
            // Arrange
            _weatherStation.RegisterDisplay(_mobileApp);
            _weatherStation.RegisterDisplay(_website);

            // Act
            _weatherStation.RemoveDisplay(_mobileApp);

            // Assert
            Assert.That(_weatherStation.DisplayCount, Is.EqualTo(1),
                "Weather station should have one display after removal");
        }

        /// <summary>
        /// Tests that setting weather data notifies all registered observers.
        /// 
        /// This is the core Observer pattern behavior - notification of all observers:
        /// - Subject notifies all registered observers when state changes
        /// - Each observer receives the updated data
        /// - All observers get the same data consistently
        /// 
        /// Required implementation:
        /// - SetWeatherData method that updates internal state and notifies observers
        /// - NotifyAll or similar method that calls Update on each registered observer
        /// - Observer Update method that receives and stores the new data
        /// </summary>
        [Test]
        public void SetWeatherData_ShouldNotifyAllDisplays()
        {
            // Arrange
            _weatherStation.RegisterDisplay(_mobileApp);
            _weatherStation.RegisterDisplay(_website);
            _weatherStation.RegisterDisplay(_billboard);

            // Act
            _weatherStation.SetWeatherData(75.5f, 65.0f, 30.2f);

            // Assert - Test that each display received the correct data
            Assert.That(_mobileApp.Temperature, Is.EqualTo(75.5f), "Mobile app should receive temperature");
            Assert.That(_mobileApp.Humidity, Is.EqualTo(65.0f), "Mobile app should receive humidity");
            Assert.That(_mobileApp.Pressure, Is.EqualTo(30.2f), "Mobile app should receive pressure");
            
            Assert.That(_website.Temperature, Is.EqualTo(75.5f), "Website should receive temperature");
            Assert.That(_website.Humidity, Is.EqualTo(65.0f), "Website should receive humidity");
            Assert.That(_website.Pressure, Is.EqualTo(30.2f), "Website should receive pressure");
            
            Assert.That(_billboard.Temperature, Is.EqualTo(75.5f), "Billboard should receive temperature");
            Assert.That(_billboard.Humidity, Is.EqualTo(65.0f), "Billboard should receive humidity");
            Assert.That(_billboard.Pressure, Is.EqualTo(30.2f), "Billboard should receive pressure");
        }

        /// <summary>
        /// Tests that setting weather data with no observers doesn't cause errors.
        /// 
        /// This verifies robust handling of edge cases:
        /// - Subject can update state even with no observers
        /// - No exceptions occur when notifying an empty observer list
        /// 
        /// Required implementation:
        /// - Safe handling of empty observer list in notification method
        /// - Graceful degradation when no observers are present
        /// </summary>
        /// <summary>
        /// Tests that setting weather data with no displays doesn't throw exception.
        /// </summary>
        [Test]
        public void SetWeatherData_WithNoDisplays_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _weatherStation.SetWeatherData(70.0f, 60.0f, 29.8f),
                "Setting weather data with no displays should not throw an exception");
        }

        /// <summary>
        /// Tests that removed displays don't receive weather updates.
        /// </summary>
        [Test]
        public void SetWeatherData_AfterRemovingDisplay_ShouldNotNotifyRemovedDisplay()
        {
            // Arrange
            _weatherStation.RegisterDisplay(_mobileApp);
            _weatherStation.RegisterDisplay(_website);
            _weatherStation.SetWeatherData(70.0f, 60.0f, 29.8f);
            
            var mobileAppFirstTemp = _mobileApp.Temperature;
            var websiteFirstTemp = _website.Temperature;

            // Act
            _weatherStation.RemoveDisplay(_mobileApp);
            _weatherStation.SetWeatherData(80.0f, 70.0f, 31.0f);

            // Assert
            Assert.That(_mobileApp.Temperature, Is.EqualTo(mobileAppFirstTemp),
                "Mobile app should not receive updates after being removed");
            
            Assert.That(_website.Temperature, Is.EqualTo(80.0f),
                "Website should still receive updates");
            Assert.That(_website.Humidity, Is.EqualTo(70.0f),
                "Website should still receive updates");
            Assert.That(_website.Pressure, Is.EqualTo(31.0f),
                "Website should still receive updates");
        }

        /// <summary>
        /// Tests that weather station stores latest weather data correctly.
        /// </summary>
        [Test]
        public void WeatherStation_ShouldStoreLatestWeatherData()
        {
            // Act
            _weatherStation.SetWeatherData(72.3f, 55.7f, 29.92f);

            // Assert
            Assert.That(_weatherStation.Temperature, Is.EqualTo(72.3f), "Temperature should be stored correctly");
            Assert.That(_weatherStation.Humidity, Is.EqualTo(55.7f), "Humidity should be stored correctly");
            Assert.That(_weatherStation.Pressure, Is.EqualTo(29.92f), "Pressure should be stored correctly");
        }

        /// <summary>
        /// Tests that registering null display doesn't add to list.
        /// </summary>
        [Test]
        public void RegisterDisplay_WithNullDisplay_ShouldNotAddToList()
        {
            // Act
            _weatherStation.RegisterDisplay(null!);

            // Assert
            Assert.That(_weatherStation.DisplayCount, Is.EqualTo(0),
                "Weather station should not register null displays");
        }

        /// <summary>
        /// Tests that display implementations have correct name properties.
        /// </summary>
        [Test]
        public void DisplayImplementations_ShouldHaveCorrectNames()
        {
            // Test that each display has the correct name
            Assert.That(_mobileApp.Name, Is.EqualTo("Mobile App"));
            Assert.That(_website.Name, Is.EqualTo("Website"));
            Assert.That(_billboard.Name, Is.EqualTo("Digital Billboard"));
        }

        /// <summary>
        /// Tests that multiple weather updates notify displays each time.
        /// </summary>
        [Test]
        public void MultipleWeatherUpdates_ShouldUpdateDisplaysEachTime()
        {
            // Arrange
            _weatherStation.RegisterDisplay(_mobileApp);

            // Act
            _weatherStation.SetWeatherData(70.0f, 60.0f, 30.0f);
            var firstTemp = _mobileApp.Temperature;
            var firstHumidity = _mobileApp.Humidity;
            
            _weatherStation.SetWeatherData(75.0f, 65.0f, 30.5f);
            var secondTemp = _mobileApp.Temperature;
            var secondHumidity = _mobileApp.Humidity;

            // Assert
            Assert.That(firstTemp, Is.EqualTo(70.0f));
            Assert.That(firstHumidity, Is.EqualTo(60.0f));
            Assert.That(secondTemp, Is.EqualTo(75.0f));
            Assert.That(secondHumidity, Is.EqualTo(65.0f));
            Assert.That(firstTemp, Is.Not.EqualTo(secondTemp));
        }
    }
}
