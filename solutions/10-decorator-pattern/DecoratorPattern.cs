/*
Decorator Pattern

Summary:
The Decorator Pattern attaches additional responsibilities to an object dynamically without altering its structure. It provides a flexible alternative to subclassing for extending functionality by wrapping objects with decorator classes that add new behaviors.

Problem to Solve:
In a coffee shop ordering system, customers want to customize their beverages with various add-ons like milk, sugar, and cream. The challenge is to calculate the final price and description dynamically based on the selected combination of add-ons, without creating separate classes for every possible combination of coffee and condiments.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Structural
{
    /// <summary>
    /// Available condiment types for decorating beverages
    /// </summary>
    public enum CondimentType
    {
        Milk,
        Sugar,
        Cream
    }

    /// <summary>
    /// Base component interface for beverages that can be decorated with additional features
    /// </summary>
    public interface IBeverage
    {
        string GetDescription();
        decimal GetCost();
    }

    public class BasicCoffee : IBeverage
    {
        public string GetDescription()
        {
            return "Basic Coffee";
        }

        public decimal GetCost()
        {
            return 0.89m;
        }
    }

    public abstract class CondimentDecorator : IBeverage
    {
        protected IBeverage _beverage;

        public CondimentDecorator(IBeverage beverage)
        {
            _beverage = beverage;
        }

        public abstract string GetDescription();
        public abstract decimal GetCost();
    }

    public class MilkDecorator : CondimentDecorator
    {
        public MilkDecorator(IBeverage beverage) : base(beverage) { }

        public override string GetDescription()
        {
            return _beverage.GetDescription() + ", Milk";
        }

        public override decimal GetCost()
        {
            return _beverage.GetCost() + 0.10m;
        }
    }

    public class SugarDecorator : CondimentDecorator
    {
        public SugarDecorator(IBeverage beverage) : base(beverage) { }

        public override string GetDescription()
        {
            return _beverage.GetDescription() + ", Sugar";
        }

        public override decimal GetCost()
        {
            return _beverage.GetCost() + 0.05m;
        }
    }

    public class CreamDecorator : CondimentDecorator
    {
        public CreamDecorator(IBeverage beverage) : base(beverage) { }

        public override string GetDescription()
        {
            return _beverage.GetDescription() + ", Cream";
        }

        public override decimal GetCost()
        {
            return _beverage.GetCost() + 0.15m;
        }
    }

    /// <summary>
    /// Creates a beverage with decorator pattern implementation and condiment factory
    /// </summary>
    public static class DecoratorPattern
    {
        public static IBeverage Create()
        {
            return new BasicCoffee();
        }

        public static IBeverage AddCondiment(IBeverage beverage, CondimentType condimentType)
        {
            if (beverage == null)
                throw new ArgumentNullException(nameof(beverage));

            return condimentType switch
            {
                CondimentType.Milk => new MilkDecorator(beverage),
                CondimentType.Sugar => new SugarDecorator(beverage),
                CondimentType.Cream => new CreamDecorator(beverage),
                _ => throw new ArgumentOutOfRangeException(nameof(condimentType), condimentType, null)
            };
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Concrete component:
       - BasicCoffee class implementing IBeverage
       - GetDescription() returns "Basic Coffee"
       - GetCost() returns 0.89m as base coffee price

    2. Decorator base class:
       - CondimentDecorator abstract class implementing IBeverage
       - Contains reference to wrapped IBeverage component
       - Provides base structure for all concrete decorators

    3. Concrete decorators:
       - MilkDecorator: Adds ", Milk" to description and 0.10m to cost
       - SugarDecorator: Adds ", Sugar" to description and 0.05m to cost
       - CreamDecorator: Adds ", Cream" to description and 0.15m to cost
       - Each decorator wraps another IBeverage and enhances its behavior

    4. Factory method implementation:
       - Create() returns basic coffee instance
       - AddCondiment(beverage, condimentType) wraps beverage with appropriate decorator based on enum
       - Use switch expression to determine which decorator to create based on CondimentType
       - Enable dynamic composition of beverage features through enum-based factory method

    5. Decorator pattern principles:
       - Each decorator should take IBeverage constructor parameter
       - Decorators should delegate to wrapped component and add their own behavior
       - All decorators return IBeverage interface to maintain composability
       - Factory method uses enum to determine which condiment decorator to apply

    The tests will verify that:
    - Basic beverage can be decorated with multiple condiments using enum-based factory method
    - Each decorator adds its cost and description to the total
    - Decorators can be stacked in any order using factory method chaining with different enum values
    - Final cost and description reflect all applied decorations
    - Factory method eliminates need for direct decorator instantiation
    - Enum provides type-safe condiment selection
    */
}
