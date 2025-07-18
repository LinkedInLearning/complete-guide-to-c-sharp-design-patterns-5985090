using System;
using NUnit.Framework;

using DotNetDesignPatterns.Patterns.Structural;

namespace DotNetDesignPatterns.Tests.Patterns.Structural
{
    /// <summary>
    /// Tests for the Composite Pattern implementation.
    /// 
    /// The Composite pattern composes objects into tree structures to represent
    /// part-whole hierarchies, allowing uniform treatment of individual objects
    /// and compositions of objects. It enables clients to treat single objects and composites uniformly.
    /// 
    /// Required implementation components:
    /// 1. Component interface (IOrgComponent) that defines common operations for all objects
    /// 2. Leaf classes (Employee) that represent individual objects with no children
    /// 3. Composite classes (Department) that can contain other components
    /// 4. Tree traversal logic that handles hierarchical operations
    /// 5. Factory method to create pre-configured organizational structures
    /// 
    /// This implementation demonstrates an organizational hierarchy where departments
    /// can contain employees and other departments uniformly.
    /// </summary>
    [TestFixture]
    public class CompositePatternTests
    {
        /// <summary>
        /// Root organization component representing the entire hierarchy.
        /// </summary>
        private IOrgComponent _organization = null!;

        /// <summary>
        /// Setup method that creates a pre-configured organizational structure.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // ALWAYS use the factory method - single entry point
            _organization = CompositePattern.CreateOrganization();
        }

        /// <summary>
        /// Tests that the factory creates a valid organization structure.
        /// </summary>
        [Test]
        public void CreateOrganization_ShouldReturnValidOrganization()
        {
            // Arrange & Act
            var organization = CompositePattern.CreateOrganization();

            // Assert
            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.GetDetails(), Is.Not.Null.And.Not.Empty);
        }

        /// <summary>
        /// Tests that the organization implements the composite interface correctly.
        /// </summary>
        [Test]
        public void Organization_ShouldImplementIOrgComponent()
        {
            // Arrange & Act & Assert
            Assert.That(_organization, Is.InstanceOf<IOrgComponent>());
        }

        /// <summary>
        /// Tests that the organization contains the expected hierarchical structure.
        /// </summary>
        [Test]
        public void Organization_ShouldHaveExpectedStructure()
        {
            // Act
            var details = _organization.GetDetails();

            // Assert - Test the specific structure created by factory
            Assert.That(details, Does.Contain("Company"), "Should contain company");
            Assert.That(details, Does.Contain("Development"), "Should contain development department");
            Assert.That(details, Does.Contain("Alice"), "Should contain Alice");
            Assert.That(details, Does.Contain("Developer"), "Should contain Developer role");
            Assert.That(details, Does.Contain("Bob"), "Should contain Bob");
            Assert.That(details, Does.Contain("Tester"), "Should contain Tester role");
            Assert.That(details, Does.Contain("Charlie"), "Should contain Charlie");
            Assert.That(details, Does.Contain("CEO"), "Should contain CEO role");
        }

        /// <summary>
        /// Tests that the organization displays proper hierarchical indentation.
        /// </summary>
        [Test]
        public void Organization_ShouldShowHierarchicalStructure()
        {
            // Act
            var details = _organization.GetDetails();

            // Assert - Test hierarchical indentation exists
            Assert.That(details, Does.Contain("Department: Company"), "Should show company as department");
            Assert.That(details, Does.Contain("Department: Development"), "Should show development as department");
            
            // Check that nested content is indented (has spaces at start of lines)
            var lines = details.Split('\n');
            var hasIndentedContent = false;
            foreach (var line in lines)
            {
                if (line.StartsWith("  ") && !string.IsNullOrWhiteSpace(line))
                {
                    hasIndentedContent = true;
                    break;
                }
            }
            Assert.That(hasIndentedContent, Is.True, "Should have indented content showing hierarchy");
        }

        /// <summary>
        /// Tests that the Composite pattern treats all components uniformly.
        /// </summary>
        /// - Uniform GetDetails method behavior across all component types
        /// - Transparent handling of complex hierarchies
        /// </summary>
        [Test]
        public void CompositePattern_ShouldTreatAllComponentsUniformly()
        {
            // Arrange
            var organization = CompositePattern.CreateOrganization();

            // Act & Assert - Should be able to call GetDetails on any component
            Assert.That(() => organization.GetDetails(), Throws.Nothing, "Should be able to call GetDetails on organization");
            Assert.That(organization.GetDetails(), Is.Not.Null.And.Not.Empty, "GetDetails should return meaningful content");
        }

        [Test]
        public void Organization_ShouldContainAllExpectedEmployees()
        {
            // Act
            var details = _organization.GetDetails();

            // Assert - Verify all employees are present with their roles
            Assert.That(details, Does.Match(@"Alice.*Developer"), "Alice should be shown as Developer");
            Assert.That(details, Does.Match(@"Bob.*Tester"), "Bob should be shown as Tester");
            Assert.That(details, Does.Match(@"Charlie.*CEO"), "Charlie should be shown as CEO");
        }

        [Test]
        public void Organization_ShouldShowProperCompositeStructure()
        {
            // Act
            var details = _organization.GetDetails();

            // Assert - Development department should contain Alice and Bob
            var lines = details.Split('\n');
            var developmentFound = false;
            var aliceUnderDevelopment = false;
            var bobUnderDevelopment = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Department: Development"))
                {
                    developmentFound = true;
                    // Check subsequent indented lines for Alice and Bob
                    for (int j = i + 1; j < lines.Length && lines[j].StartsWith("  "); j++)
                    {
                        if (lines[j].Contains("Alice"))
                            aliceUnderDevelopment = true;
                        if (lines[j].Contains("Bob"))
                            bobUnderDevelopment = true;
                    }
                    break;
                }
            }

            Assert.That(developmentFound, Is.True, "Should find Development department");
            Assert.That(aliceUnderDevelopment, Is.True, "Alice should be under Development department");
            Assert.That(bobUnderDevelopment, Is.True, "Bob should be under Development department");
        }
    }
}
