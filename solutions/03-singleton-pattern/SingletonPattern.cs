/*
Singleton Pattern

Summary:
The Singleton Pattern ensures that a class has only one instance throughout the application's lifetime and provides a global point of access to that instance. It controls object creation and prevents multiple instantiations while maintaining thread safety in concurrent environments.

Problem to Solve:
In application configuration management, multiple components need access to the same configuration settings, but creating multiple configuration manager instances would waste resources and potentially cause inconsistencies. The challenge is to ensure a single, globally accessible configuration manager that is thread-safe and lazily initialized.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DotNetDesignPatterns.Patterns.Creational
{
    /// <summary>
    /// Interface for accessing application configuration
    /// </summary>
    public interface IConfigurationManager
    {
        void SetSetting(string key, string value);
        string? GetSetting(string key);
    }

    internal class ConfigurationManager : IConfigurationManager
    {

        private static ConfigurationManager? _instance;

        private static readonly object _lock = new object();

        private ConfigurationManager()
        {
            Settings = new Dictionary<string, string>();
        }

        public static ConfigurationManager Instance
        {

            get
            {

                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigurationManager();
                        }
                    }
                }

                return _instance;

            }

        }

        private Dictionary<string, string> Settings { get; set; }

        public void SetSetting(string key, string value)
        {
            Settings[key] = value;
        }

        public string? GetSetting(string key)
        {
            return Settings.TryGetValue(key, out string? value) ? value : null;
        }

        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }




    }



    /// <summary>
    /// Creates and returns the singleton configuration manager
    /// </summary>
    public static class SingletonPattern
    {
        public static IConfigurationManager Create()
        {
            return ConfigurationManager.Instance;
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. ConfigurationManager class implementing IConfigurationManager:
       - Private constructor to prevent direct instantiation
       - Private static instance variable to hold the singleton
       - Thread-safe double-check locking for instance creation
       - Internal dictionary for storing configuration key-value pairs

    2. Singleton access mechanism:
       - Public static property or method to access the single instance
       - Lazy initialization (instance created only when first accessed)
       - Multiple calls should always return the same object reference
       - Thread-safe implementation for concurrent access

    3. Configuration management:
       - SetSetting(key, value) stores configuration in internal dictionary
       - GetSetting(key) retrieves configuration value or returns null
       - State persists across all access points throughout application lifetime

    4. Testing support:
       - Public static ResetInstance() method to clear singleton for testing
       - This method is required by tests to ensure clean state between test runs
       - Should reset both the instance and any stored configuration data

    The tests will verify that:
    - Multiple Create() calls return the same object reference
    - Thread safety works correctly in concurrent scenarios
    - Configuration settings persist across different access points
    - ResetInstance() properly clears singleton state for testing
    */
}
