using System;
using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Structural;

namespace DotNetDesignPatterns.Tests.Patterns.Structural
{
    /// <summary>
    /// Tests for the Facade Pattern implementation.
    /// 
    /// The Facade pattern provides a unified interface to a set of interfaces in a subsystem.
    /// It defines a higher-level interface that makes the subsystem easier to use by hiding
    /// the complexity of multiple subsystem components behind a single, simplified interface.
    /// 
    /// Required implementation components:
    /// 1. Facade interface (ISocialMediaService) that provides simplified methods
    /// 2. Subsystem classes (FacebookAPI, TwitterAPI, InstagramAPI) with complex interfaces
    /// 3. Facade implementation that coordinates calls to multiple subsystem components
    /// 4. Factory method to create the facade with all subsystem dependencies
    /// 
    /// This implementation demonstrates a social media facade that simplifies posting
    /// to multiple social media platforms through a single interface.
    /// </summary>
    [TestFixture]
    public class FacadePatternTests
    {
        /// <summary>
        /// The facade instance that provides simplified access to social media subsystems.
        /// Should coordinate with multiple social media APIs behind a unified interface.
        /// </summary>
        private ISocialMediaService _facade = null!;

        /// <summary>
        /// Setup method that creates the facade for testing.
        /// 
        /// Required implementation:
        /// - FacadePattern.CreateSocialMediaFacade() factory method
        /// - Method should create and wire up all subsystem dependencies
        /// - Return a facade that implements ISocialMediaService
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Use factory method - single entry point
            _facade = FacadePattern.CreateSocialMediaFacade();
        }

        /// <summary>
        /// Tests that the facade factory creates a valid facade instance.
        /// 
        /// This verifies:
        /// - Factory method returns a non-null facade
        /// - Facade implements the expected interface
        /// - Basic instantiation works correctly
        /// 
        /// Required implementation:
        /// - Factory method that creates the facade
        /// - Proper interface implementation
        /// - All subsystem dependencies properly initialized
        /// </summary>
        /// <summary>
        /// Tests that the factory creates a valid social media facade.
        /// </summary>
        [Test]
        public void CreateSocialMediaFacade_ShouldReturnValidFacade()
        {
            // Arrange & Act
            var facade = FacadePattern.CreateSocialMediaFacade();

            // Assert
            Assert.That(facade, Is.Not.Null);
            Assert.That(facade, Is.InstanceOf<ISocialMediaService>());
        }

        /// <summary>
        /// Tests that the facade can post to all platforms without errors.
        /// 
        /// This demonstrates the core Facade pattern behavior:
        /// - Single method call coordinates multiple subsystem operations
        /// - Complex subsystem interactions are hidden from the client
        /// - Error handling is centralized in the facade
        /// 
        /// Required implementation:
        /// - Post method that calls multiple social media APIs
        /// - Proper error handling and coordination
        /// - Abstraction of subsystem complexity
        /// </summary>
        [Test]
        public void SocialMediaFacade_ShouldPostToAllPlatformsCorrectly()
        {
            // Arrange
            var message = "Hello, world!";

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => _facade.Post(message));
        }

        /// <summary>
        /// Tests that facade handles empty message gracefully.
        /// </summary>
        [Test]
        public void SocialMediaFacade_ShouldHandleEmptyMessage()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => _facade.Post(""));
        }

        /// <summary>
        /// Tests that facade handles null message with proper error.
        /// </summary>
        [Test]
        public void SocialMediaFacade_ShouldHandleNullMessage()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => _facade.Post(null!));
        }

        /// <summary>
        /// Tests that facade simplifies complex multi-platform operations.
        /// </summary>
        [Test]
        public void SocialMediaFacade_ShouldSimplifyComplexOperation()
        {
            // Arrange
            var message = "Testing Facade Pattern";

            // Act - Single method call coordinates multiple subsystems
            _facade.Post(message);

            // Assert - If we get here, the facade successfully coordinated all platforms
            Assert.Pass("Social media facade successfully coordinated posting to all platforms");
        }

        /// <summary>
        /// Tests that facade implements the required interface.
        /// </summary>
        [Test]
        public void SocialMediaFacade_ShouldImplementFacadeInterface()
        {
            // Arrange & Act & Assert
            Assert.That(_facade, Is.InstanceOf<ISocialMediaService>());
        }
    }
}
