using System;
using NUnit.Framework;

// Use conditional imports based on project

using DotNetDesignPatterns.Patterns.Structural;


namespace DotNetDesignPatterns.Tests.Patterns.Structural
{
    /// <summary>
    /// Tests for the Decorator Pattern implementation.
    /// 
    /// The Decorator pattern allows behavior to be added to objects dynamically without
    /// altering their structure. It provides a flexible alternative to subclassing for
    /// extending functionality by wrapping objects with decorator classes.
    /// 
    /// Required implementation components:
    /// 1. Component interface (IBeverage) that defines the common interface
    /// 2. Concrete component (Coffee) that implements the base functionality
    /// 3. Base decorator class that implements the component interface and wraps a component
    /// 4. Concrete decorators (MilkDecorator, SugarDecorator, CreamDecorator) that add specific functionality
    /// 5. Enum-based factory method to create decorators without using 'new' keyword
    /// 
    /// This implementation demonstrates decorating a coffee beverage with various add-ons,
    /// where each decorator can modify the description and cost while maintaining the interface.
    /// Enum-based factory method ensures type-safe condiment selection and eliminates hardcoded methods.
    /// These tests verify the Decorator pattern implementation works correctly.
    /// </summary>
    [TestFixture]
    public class DecoratorPatternTests
    {
        // Use interfaces, not concrete types
        private IBeverage _coffee = null!;

        [SetUp]
        public void Setup()
        {
            // Use the factory method to get the base coffee - will throw NotImplementedException if not implemented
            _coffee = DecoratorPattern.Create();
        }
        
        /// <summary>
        /// Tests that basic coffee returns correct description and cost.
        /// </summary>
        [Test]
        public void Coffee_ShouldReturnCorrectDescriptionAndCost()
        {
            // Act & Assert
            Assert.That(_coffee.GetDescription(), Is.EqualTo("Basic Coffee"));
            Assert.That(_coffee.GetCost(), Is.EqualTo(0.89m));
        }

        /// <summary>
        /// Tests that adding milk decorator modifies description and cost correctly.
        /// </summary>
        [Test]
        public void CoffeeWithMilk_ShouldAddMilkToDescriptionAndCost()
        {
            // Arrange - Create decorator using enum-based factory method
            var coffeeWithMilk = DecoratorPattern.AddCondiment(_coffee, CondimentType.Milk);

            // Act & Assert
            Assert.That(coffeeWithMilk.GetDescription(), Is.EqualTo("Basic Coffee, Milk"));
            Assert.That(Math.Round(coffeeWithMilk.GetCost(), 2), Is.EqualTo(0.99m)); // 0.89 + 0.10
        }

        /// <summary>
        /// Tests that multiple decorators can be chained together correctly.
        /// </summary>
        [Test]
        public void CoffeeWithMilkAndSugar_ShouldAddBothToDescriptionAndCost()
        {
            // Arrange - Add multiple condiments using enum-based factory methods
            var coffeeWithMilk = DecoratorPattern.AddCondiment(_coffee, CondimentType.Milk);
            var coffeeWithMilkAndSugar = DecoratorPattern.AddCondiment(coffeeWithMilk, CondimentType.Sugar);

            // Act & Assert
            Assert.That(coffeeWithMilkAndSugar.GetDescription(), Is.EqualTo("Basic Coffee, Milk, Sugar"));
            Assert.That(Math.Round(coffeeWithMilkAndSugar.GetCost(), 2), Is.EqualTo(1.04m)); // 0.89 + 0.10 + 0.05
        }

        /// <summary>
        /// Tests that complex beverages with all decorators calculate correctly.
        /// </summary>
        [Test]
        public void ComplexBeverage_WithAllDecorators_ShouldCalculateCorrectly()
        {
            // Arrange - Create complex beverage with multiple condiments using enum-based factory methods
            var withMilk = DecoratorPattern.AddCondiment(_coffee, CondimentType.Milk);
            var withMilkAndSugar = DecoratorPattern.AddCondiment(withMilk, CondimentType.Sugar);
            var complexBeverage = DecoratorPattern.AddCondiment(withMilkAndSugar, CondimentType.Cream);

            // Act & Assert
            Assert.That(complexBeverage.GetDescription(), 
                Is.EqualTo("Basic Coffee, Milk, Sugar, Cream"));
            Assert.That(Math.Round(complexBeverage.GetCost(), 2), Is.EqualTo(1.19m)); // 0.89 + 0.10 + 0.05 + 0.15
        }

        /// <summary>
        /// Tests that decorators can be stacked in any order with same result.
        /// </summary>
        [Test]
        public void Decorators_ShouldBeStackableInAnyOrder()
        {
            // Arrange - Different stacking orders using enum-based factory methods
            var milkFirst = DecoratorPattern.AddCondiment(_coffee, CondimentType.Milk);
            var milkThenSugar = DecoratorPattern.AddCondiment(milkFirst, CondimentType.Sugar);
            
            var sugarFirst = DecoratorPattern.AddCondiment(_coffee, CondimentType.Sugar);
            var sugarThenMilk = DecoratorPattern.AddCondiment(sugarFirst, CondimentType.Milk);

            // Act & Assert
            Assert.That(Math.Round(sugarThenMilk.GetCost(), 2), Is.EqualTo(Math.Round(milkThenSugar.GetCost(), 2)),
                "Cost should be the same regardless of condiment order");
            
            // Both should have the same cost and similar descriptions (order may differ)
            Assert.That(sugarThenMilk.GetDescription().Contains("Sugar"), Is.True, "Description should contain Sugar");
            Assert.That(sugarThenMilk.GetDescription().Contains("Milk"), Is.True, "Description should contain Milk");
            Assert.That(milkThenSugar.GetDescription().Contains("Sugar"), Is.True, "Description should contain Sugar");
            Assert.That(milkThenSugar.GetDescription().Contains("Milk"), Is.True, "Description should contain Milk");
        }

        /// <summary>
        /// Tests that decorator throws exception when beverage is null.
        /// </summary>
        [Test]
        public void CondimentDecorator_WithNullBeverage_ShouldThrowException()
        {
            // Act & Assert - Enum-based factory method should handle null validation
            Assert.Throws<ArgumentNullException>(() => DecoratorPattern.AddCondiment(null!, CondimentType.Milk));
            Assert.Throws<ArgumentNullException>(() => DecoratorPattern.AddCondiment(null!, CondimentType.Sugar));
            Assert.Throws<ArgumentNullException>(() => DecoratorPattern.AddCondiment(null!, CondimentType.Cream));
        }
    }
}

