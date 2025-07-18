/*
Prototype Pattern - Configuration Profile Example

Summary:
The Prototype Pattern creates new objects by cloning existing instances rather than creating them from scratch. 
This is useful when you want to create variations of complex objects without going through expensive setup processes.

Problem to Solve:
In an application settings system, users want to create new configuration profiles based on existing ones.
Instead of setting up each profile from scratch, they want to clone an existing profile and make adjustments.

Example Scenario:
- A gaming application has different performance profiles (Low, Medium, High)
- Users want to create custom profiles based on existing ones
- Each profile has multiple settings that would be tedious to configure manually
*/

using System;

namespace DotNetDesignPatterns.Patterns.Creational
{
    /// <summary>
    /// Interface for objects that can be cloned
    /// </summary>
    public interface IPrototype<T>
    {
        T Clone();
    }

    /// <summary>
    /// Represents a configuration profile that can be cloned
    /// </summary>
    public interface IConfigurationProfile : IPrototype<IConfigurationProfile>
    {
        string Name { get; set; }
        int GraphicsQuality { get; set; }  // 1-10 scale
        int SoundVolume { get; set; }      // 0-100 scale
        bool EnableVSync { get; set; }
        string GetSummary();
    }

    /// <summary>
    /// Factory for creating configuration profiles
    /// </summary>
    public static class PrototypePattern
    {
        /// <summary>
        /// Creates a default "Medium" performance profile
        /// </summary>
        /// <returns>A configuration profile with medium settings</returns>
        public static IConfigurationProfile CreateDefaultProfile()
        {
            throw new NotImplementedException("Implement the ConfigurationProfile class and return a default instance");
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. ConfigurationProfile class implementing IConfigurationProfile:
       - Should have all the interface properties (Name, GraphicsQuality, SoundVolume, EnableVSync)
       - Implement Clone() method that creates a new ConfigurationProfile with the same values
       - Implement GetSummary() method that returns "Profile: [name] - Graphics: [quality], Sound: [volume], VSync: [true/false]"
       - Constructor should accept all parameters

    2. Update PrototypePattern.CreateDefaultProfile():
       - Return a new ConfigurationProfile with Name="Medium", GraphicsQuality=5, SoundVolume=50, EnableVSync=true

    The tests will verify that:
    - Cloned profiles are independent copies (changes don't affect originals)
    - All properties are properly copied during cloning
    - GetSummary() method returns correctly formatted information
    */
}
