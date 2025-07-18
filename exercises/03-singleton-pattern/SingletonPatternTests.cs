using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Reflection;

using DotNetDesignPatterns.Patterns.Creational;


namespace DotNetDesignPatterns.Tests.Patterns.Creational
{
    /// <summary>
    /// Tests for the Singleton Pattern implementation.
    /// 
    /// The Singleton pattern ensures a class has only one instance and provides a global
    /// point of access to that instance. It's commonly used for configuration managers,
    /// loggers, and other shared resources.
    /// 
    /// Required implementation components:
    /// 1. Private constructor to prevent external instantiation
    /// 2. Static instance variable to hold the single instance
    /// 3. Public static method or property to access the instance
    /// 4. Thread-safe implementation to handle concurrent access
    /// 5. Factory method for testability and abstraction
    /// 
    /// These tests verify the Singleton implementation works correctly and is thread-safe.
    /// They will run against either the Examples or Solutions project depending on configuration.
    /// </summary>
    [TestFixture]
    public class SingletonPatternTests
    {
        private IConfigurationManager _configManager = null!;
        private Type _configManagerType = null!;
        
        [SetUp]
        public void Setup()
        {
            // Get the config manager instance from the factory
            _configManager = SingletonPattern.Create();
            
            // Find the concrete type to reset it between tests
            _configManagerType = _configManager.GetType();
            
            // Reset the singleton instance before each test
            var resetMethod = _configManagerType.GetMethod("ResetInstance", BindingFlags.Public | BindingFlags.Static);
            resetMethod?.Invoke(null, null);
        }

        [Test]
        public void Instance_ShouldReturnSameInstanceEveryTime()
        {
            // Arrange & Act
            var instance1 = SingletonPattern.Create();
            var instance2 = SingletonPattern.Create();

            // Assert
            Assert.That(instance2, Is.SameAs(instance1), 
                "Multiple calls to Create() should return the same object reference");
        }

        [Test]
        public void Instance_ShouldBeThreadSafe()
        {
            // Arrange
            IConfigurationManager? instance1 = null;
            IConfigurationManager? instance2 = null;
            var task1Complete = false;
            var task2Complete = false;

            // Act
            var task1 = Task.Run(() =>
            {
                instance1 = SingletonPattern.Create();
                task1Complete = true;
            });

            var task2 = Task.Run(() =>
            {
                instance2 = SingletonPattern.Create();
                task2Complete = true;
            });

            Task.WaitAll(task1, task2);

            // Assert
            Assert.That(task1Complete, Is.True, "Task 1 should complete");
            Assert.That(task2Complete, Is.True, "Task 2 should complete");
            Assert.That(instance1, Is.Not.Null, "Instance 1 should not be null");
            Assert.That(instance2, Is.Not.Null, "Instance 2 should not be null");
            Assert.That(instance2, Is.SameAs(instance1), 
                "Instances created from different threads should be the same");
        }

        [Test]
        public void Settings_ShouldPersistAcrossInstanceCalls()
        {
            // Arrange
            var instance1 = SingletonPattern.Create();
            instance1.SetSetting("TestKey", "TestValue");

            // Act
            var instance2 = SingletonPattern.Create();
            var retrievedValue = instance2.GetSetting("TestKey");

            // Assert
            Assert.That(retrievedValue, Is.EqualTo("TestValue"), 
                "Settings should persist across instance calls");
        }

        [Test]
        public void SetSetting_ShouldStoreValue()
        {
            // Arrange
            var instance = SingletonPattern.Create();

            // Act
            instance.SetSetting("DatabaseConnection", "Server=localhost;Database=Test");
            var result = instance.GetSetting("DatabaseConnection");

            // Assert
            Assert.That(result, Is.EqualTo("Server=localhost;Database=Test"),
                "SetSetting should store the value correctly");
        }

        [Test]
        public void GetSetting_WithNonExistentKey_ShouldReturnNull()
        {
            // Arrange
            var instance = SingletonPattern.Create();

            // Act
            var result = instance.GetSetting("NonExistentKey");

            // Assert
            Assert.That(result, Is.Null,
                "GetSetting with non-existent key should return null");
        }
    }
}
