using System;
using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Creational;

namespace DotNetDesignPatterns.Tests.Patterns.Creational
{
    /// <summary>
    /// Tests for the Prototype Pattern implementation using Configuration Profiles.
    /// 
    /// The Prototype pattern creates objects by cloning existing instances (prototypes)
    /// rather than instantiating new objects from classes. This is useful when object
    /// creation is expensive or when you need to create objects that are similar to existing ones.
    /// 
    /// This implementation uses a Configuration Profile example to demonstrate:
    /// 1. Basic cloning functionality for settings/configuration objects
    /// 2. Independent object instances after cloning
    /// 3. Proper copying of multiple properties (strings, integers, booleans)
    /// 4. Real-world application in settings management
    /// </summary>
    [TestFixture]
    public class PrototypePatternTests
    {
        /// <summary>
        /// Tests that the factory creates a valid default configuration profile.
        /// </summary>
        [Test]
        public void CreateDefaultProfile_ShouldReturnValidProfile()
        {
            // Arrange & Act
            var profile = PrototypePattern.CreateDefaultProfile();

            // Assert
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.Name, Is.EqualTo("Medium"));
            Assert.That(profile.GraphicsQuality, Is.EqualTo(5));
            Assert.That(profile.SoundVolume, Is.EqualTo(50));
            Assert.That(profile.EnableVSync, Is.True);
        }

        /// <summary>
        /// Tests that a configuration profile can be cloned and the clone is a separate instance.
        /// </summary>
        [Test]
        public void ConfigurationProfile_Clone_ShouldCreateIndependentInstance()
        {
            // Arrange
            var originalProfile = PrototypePattern.CreateDefaultProfile();

            // Act
            var clonedProfile = originalProfile.Clone();

            // Assert
            Assert.That(clonedProfile, Is.Not.Null);
            Assert.That(clonedProfile, Is.Not.SameAs(originalProfile), "Clone should be a different object instance");
            Assert.That(clonedProfile.Name, Is.EqualTo(originalProfile.Name), "Clone should have the same name");
            Assert.That(clonedProfile.GraphicsQuality, Is.EqualTo(originalProfile.GraphicsQuality), "Clone should have the same graphics quality");
            Assert.That(clonedProfile.SoundVolume, Is.EqualTo(originalProfile.SoundVolume), "Clone should have the same sound volume");
            Assert.That(clonedProfile.EnableVSync, Is.EqualTo(originalProfile.EnableVSync), "Clone should have the same VSync setting");
        }

        /// <summary>
        /// Tests that modifying a cloned configuration profile does not affect the original.
        /// </summary>
        [Test]
        public void ModifyingClonedProfile_ShouldNotAffectOriginal()
        {
            // Arrange
            var originalProfile = PrototypePattern.CreateDefaultProfile();
            var clonedProfile = originalProfile.Clone();

            // Act
            clonedProfile.Name = "Custom High";
            clonedProfile.GraphicsQuality = 8;
            clonedProfile.SoundVolume = 75;
            clonedProfile.EnableVSync = false;

            // Assert
            Assert.That(originalProfile.Name, Is.EqualTo("Medium"), "Original profile's name should not change");
            Assert.That(originalProfile.GraphicsQuality, Is.EqualTo(5), "Original profile's graphics quality should not change");
            Assert.That(originalProfile.SoundVolume, Is.EqualTo(50), "Original profile's sound volume should not change");
            Assert.That(originalProfile.EnableVSync, Is.True, "Original profile's VSync setting should not change");

            Assert.That(clonedProfile.Name, Is.EqualTo("Custom High"), "Cloned profile's name should be updated");
            Assert.That(clonedProfile.GraphicsQuality, Is.EqualTo(8), "Cloned profile's graphics quality should be updated");
            Assert.That(clonedProfile.SoundVolume, Is.EqualTo(75), "Cloned profile's sound volume should be updated");
            Assert.That(clonedProfile.EnableVSync, Is.False, "Cloned profile's VSync setting should be updated");
        }

        /// <summary>
        /// Tests that the GetSummary method works correctly.
        /// </summary>
        [Test]
        public void ConfigurationProfile_GetSummary_ShouldReturnFormattedString()
        {
            // Arrange
            var profile = PrototypePattern.CreateDefaultProfile();

            // Act
            var summary = profile.GetSummary();

            // Assert
            Assert.That(summary, Is.EqualTo("Profile: Medium - Graphics: 5, Sound: 50, VSync: True"));
        }

        /// <summary>
        /// Tests that multiple clones can be created and modified independently.
        /// </summary>
        [Test]
        public void MultipleClones_ShouldBeIndependent()
        {
            // Arrange
            var originalProfile = PrototypePattern.CreateDefaultProfile();

            // Act
            var clone1 = originalProfile.Clone();
            var clone2 = originalProfile.Clone();
            
            clone1.Name = "Gaming Profile";
            clone1.GraphicsQuality = 9;
            
            clone2.Name = "Power Saver";
            clone2.GraphicsQuality = 2;

            // Assert
            Assert.That(originalProfile.Name, Is.EqualTo("Medium"), "Original should be unchanged");
            Assert.That(clone1.Name, Is.EqualTo("Gaming Profile"), "Clone 1 should have its own name");
            Assert.That(clone2.Name, Is.EqualTo("Power Saver"), "Clone 2 should have its own name");
            Assert.That(clone1.GraphicsQuality, Is.EqualTo(9), "Clone 1 should have its own graphics quality");
            Assert.That(clone2.GraphicsQuality, Is.EqualTo(2), "Clone 2 should have its own graphics quality");
            Assert.That(clone1, Is.Not.SameAs(clone2), "Clones should be different instances");
        }

        /// <summary>
        /// Tests that clones can be chained (clone of a clone).
        /// </summary>
        [Test]
        public void CloneOfClone_ShouldWork()
        {
            // Arrange
            var originalProfile = PrototypePattern.CreateDefaultProfile();
            var firstClone = originalProfile.Clone();
            firstClone.Name = "First Clone";
            firstClone.GraphicsQuality = 7;

            // Act
            var secondClone = firstClone.Clone();

            // Assert
            Assert.That(secondClone, Is.Not.SameAs(firstClone), "Second clone should be different from first clone");
            Assert.That(secondClone.Name, Is.EqualTo("First Clone"), "Second clone should copy from first clone");
            Assert.That(secondClone.GraphicsQuality, Is.EqualTo(7), "Graphics quality should be preserved through cloning chain");
            Assert.That(secondClone.SoundVolume, Is.EqualTo(50), "Sound volume should be preserved through cloning chain");
            Assert.That(secondClone.EnableVSync, Is.True, "VSync setting should be preserved through cloning chain");
        }

        /// <summary>
        /// Tests creating custom profiles from different starting points.
        /// </summary>
        [Test]
        public void CreateCustomProfilesFromDifferentBase_ShouldWork()
        {
            // Arrange
            var mediumProfile = PrototypePattern.CreateDefaultProfile();

            // Act
            var gamingProfile = mediumProfile.Clone();
            gamingProfile.Name = "Gaming";
            gamingProfile.GraphicsQuality = 10;
            gamingProfile.SoundVolume = 90;

            var officeProfile = mediumProfile.Clone();
            officeProfile.Name = "Office";
            officeProfile.GraphicsQuality = 3;
            officeProfile.SoundVolume = 20;
            officeProfile.EnableVSync = false;

            // Assert
            Assert.That(gamingProfile.GetSummary(), Is.EqualTo("Profile: Gaming - Graphics: 10, Sound: 90, VSync: True"));
            Assert.That(officeProfile.GetSummary(), Is.EqualTo("Profile: Office - Graphics: 3, Sound: 20, VSync: False"));
            Assert.That(mediumProfile.GetSummary(), Is.EqualTo("Profile: Medium - Graphics: 5, Sound: 50, VSync: True"));
        }
    }
}
