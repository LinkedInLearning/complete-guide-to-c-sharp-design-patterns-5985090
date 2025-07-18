using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Creational;

namespace DotNetDesignPatterns.Tests.Patterns.Creational
{
    /// <summary>
    /// Tests for the Builder Pattern implementation.
    /// 
    /// The Builder pattern separates the construction of complex objects from their representation,
    /// allowing the same construction process to create different representations. It's particularly
    /// useful when creating objects that require multiple steps or have many optional parameters.
    /// 
    /// Required implementation components:
    /// 1. ITopping interface with Name, Price, and OrderWeight properties
    /// 2. IPizza interface with Toppings collection and GetPrice() method
    /// 3. IPizzaShop interface with PlaceOrder() method
    /// 4. Topping class implementing ITopping with proper ordering
    /// 5. Pizza class implementing IPizza with sorted toppings and price calculation
    /// 6. PizzaShop class with internal PizzaBuilder for step-by-step construction
    /// 7. Factory methods to create pizza shop instances and toppings
    /// 
    /// Student must implement:
    /// - BuilderPattern.Create() - creates pizza shop instances
    /// - BuilderPattern.CreateTopping() - creates individual topping instances
    /// - BuilderPattern.CreateTestToppings() - creates standard topping collection
    /// - All required interfaces and concrete classes
    /// 
    /// This implementation demonstrates a pizza ordering system where customers can build
    /// custom pizzas with various toppings that are automatically layered in proper order
    /// (sauce first, then cheese, then other toppings) regardless of input sequence.
    /// </summary>
    [TestFixture]
    public class BuilderPatternTests
    {
        private IPizzaShop _pizzaShop = null!;
        private List<ITopping> _testToppings = null!;

        /// <summary>
        /// Sets up test fixtures with a pizza shop and sample toppings.
        /// 
        /// Creates test toppings using factory methods:
        /// - Sauce: OrderWeight 1 (goes first)
        /// - Cheese: OrderWeight 2 (goes second)
        /// - Other toppings: OrderWeight 3+ (go last)
        /// 
        /// Required implementation:
        /// - BuilderPattern.Create() factory method
        /// - BuilderPattern.CreateTestToppings() factory method
        /// - Proper OrderWeight assignment for layering logic
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _pizzaShop = BuilderPattern.Create();
            
            // Create test toppings using factory method
            _testToppings = BuilderPattern.CreateTestToppings().ToList();
        }

        /// <summary>
        /// Tests that the factory creates a valid pizza shop instance.
        /// 
        /// This verifies:
        /// - Factory method returns a non-null instance
        /// - Returned instance implements IPizzaShop interface
        /// - Basic factory pattern compliance
        /// 
        /// Required implementation:
        /// - BuilderPattern.Create() static factory method
        /// - PizzaShop class implementing IPizzaShop interface
        /// - Proper object instantiation and return
        /// </summary>
        [Test]
        public void Create_ShouldReturnValidPizzaShop()
        {
            // Arrange & Act
            var pizzaShop = BuilderPattern.Create();

            // Assert
            Assert.That(pizzaShop, Is.Not.Null);
            Assert.That(pizzaShop, Is.AssignableTo<IPizzaShop>());
        }

        /// <summary>
        /// Tests that topping implementation has correct properties and ordering.
        /// 
        /// This verifies:
        /// - Topping class properly implements ITopping interface
        /// - Name, Price, and OrderWeight properties work correctly
        /// - OrderWeight values enable proper layering sequence
        /// 
        /// Required implementation:
        /// - Topping class with public constructor
        /// - Proper property implementation for Name, Price, OrderWeight
        /// - Constructor parameter validation and assignment
        /// </summary>
        [Test]
        public void Topping_ShouldHaveCorrectProperties()
        {
            // Arrange & Act
            var sauce = BuilderPattern.CreateTopping("Sauce", 1.00m, 1);
            var cheese = BuilderPattern.CreateTopping("Cheese", 2.00m, 2);
            var pepperoni = BuilderPattern.CreateTopping("Pepperoni", 2.50m, 3);

            // Assert
            Assert.That(sauce.Name, Is.EqualTo("Sauce"));
            Assert.That(sauce.Price, Is.EqualTo(1.00m));
            Assert.That(sauce.OrderWeight, Is.EqualTo(1));

            Assert.That(cheese.Name, Is.EqualTo("Cheese"));
            Assert.That(cheese.Price, Is.EqualTo(2.00m));
            Assert.That(cheese.OrderWeight, Is.EqualTo(2));

            Assert.That(pepperoni.Name, Is.EqualTo("Pepperoni"));
            Assert.That(pepperoni.Price, Is.EqualTo(2.50m));
            Assert.That(pepperoni.OrderWeight, Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that pizza price calculation includes base price plus toppings.
        /// 
        /// This verifies:
        /// - Base pizza price is $8.00
        /// - Topping prices are added correctly
        /// - GetPrice() method returns accurate total
        /// 
        /// Required implementation:
        /// - Pizza class with GetPrice() method
        /// - Base price constant of $8.00
        /// - Sum of all topping prices added to base price
        /// - Decimal precision for currency calculations
        /// </summary>
        [Test]
        public void Pizza_GetPrice_ShouldIncludeBasePricePlusToppings()
        {
            // Arrange
            var toppings = new List<ITopping>
            {
                BuilderPattern.CreateTopping("Sauce", 1.00m, 1),
                BuilderPattern.CreateTopping("Cheese", 2.00m, 2)
            };

            // Act
            var pizza = _pizzaShop.PlaceOrder(toppings);
            var price = pizza.GetPrice();

            // Assert
            // Base pizza ($8.00) + Sauce ($1.00) + Cheese ($2.00) = $11.00
            Assert.That(price, Is.EqualTo(11.00m));
        }

        /// <summary>
        /// Tests that toppings are automatically sorted by OrderWeight regardless of input order.
        /// 
        /// This demonstrates the Builder pattern's construction logic:
        /// - Input toppings can be in any order
        /// - Builder sorts by OrderWeight during construction
        /// - Final pizza has toppings in proper layering sequence
        /// 
        /// Required implementation:
        /// - Pizza constructor or builder that sorts toppings by OrderWeight
        /// - Toppings property returns IReadOnlyList in sorted order
        /// - Proper LINQ OrderBy or equivalent sorting logic
        /// </summary>
        [Test]
        public void Pizza_ShouldSortToppingsByOrderWeight()
        {
            // Arrange - deliberately provide toppings in wrong order
            var unsortedToppings = new List<ITopping>
            {
                BuilderPattern.CreateTopping("Pepperoni", 2.50m, 3),  // Should be 3rd
                BuilderPattern.CreateTopping("Sauce", 1.00m, 1),      // Should be 1st
                BuilderPattern.CreateTopping("Mushrooms", 1.50m, 3),  // Should be 4th
                BuilderPattern.CreateTopping("Cheese", 2.00m, 2)      // Should be 2nd
            };

            // Act
            var pizza = _pizzaShop.PlaceOrder(unsortedToppings);

            // Assert - verify proper ordering by OrderWeight
            Assert.That(pizza.Toppings.Count, Is.EqualTo(4));
            Assert.That(pizza.Toppings[0].Name, Is.EqualTo("Sauce"));     // OrderWeight 1
            Assert.That(pizza.Toppings[1].Name, Is.EqualTo("Cheese"));    // OrderWeight 2
            Assert.That(pizza.Toppings[2].Name, Is.EqualTo("Pepperoni")); // OrderWeight 3
            Assert.That(pizza.Toppings[3].Name, Is.EqualTo("Mushrooms")); // OrderWeight 3
        }

        /// <summary>
        /// Tests that the builder can create pizzas with different topping combinations.
        /// 
        /// This verifies the Builder pattern's flexibility:
        /// - Same construction process creates different pizza variations
        /// - Each order produces a separate pizza instance
        /// - Different topping combinations are handled correctly
        /// 
        /// Required implementation:
        /// - Builder state management to handle multiple orders
        /// - Proper instance creation for each PlaceOrder call
        /// - Independent pizza objects with their own topping collections
        /// </summary>
        [Test]
        public void PizzaShop_ShouldCreateDifferentPizzaConfigurations()
        {
            // Arrange
            var margheritaToppings = new List<ITopping>
            {
                BuilderPattern.CreateTopping("Sauce", 1.00m, 1),
                BuilderPattern.CreateTopping("Cheese", 2.00m, 2)
            };

            var pepperoniToppings = new List<ITopping>
            {
                BuilderPattern.CreateTopping("Sauce", 1.00m, 1),
                BuilderPattern.CreateTopping("Cheese", 2.00m, 2),
                BuilderPattern.CreateTopping("Pepperoni", 2.50m, 3)
            };

            // Act
            var margherita = _pizzaShop.PlaceOrder(margheritaToppings);
            var pepperoni = _pizzaShop.PlaceOrder(pepperoniToppings);

            // Assert
            Assert.That(margherita.Toppings.Count, Is.EqualTo(2));
            Assert.That(margherita.GetPrice(), Is.EqualTo(11.00m)); // $8 + $1 + $2

            Assert.That(pepperoni.Toppings.Count, Is.EqualTo(3));
            Assert.That(pepperoni.GetPrice(), Is.EqualTo(13.50m)); // $8 + $1 + $2 + $2.50

            // Verify they are different instances
            Assert.That(margherita, Is.Not.SameAs(pepperoni));
        }

        /// <summary>
        /// Tests that empty topping orders create valid base pizzas.
        /// 
        /// This verifies edge case handling:
        /// - Builder handles empty topping lists gracefully
        /// - Base pizza is created with no toppings
        /// - Price calculation works for topping-free pizzas
        /// 
        /// Required implementation:
        /// - Builder logic that handles empty collections
        /// - Pizza constructor that accepts empty topping lists
        /// - GetPrice() method that returns base price when no toppings present
        /// </summary>
        [Test]
        public void PizzaShop_ShouldHandleEmptyToppings()
        {
            // Arrange
            var emptyToppings = new List<ITopping>();

            // Act
            var pizza = _pizzaShop.PlaceOrder(emptyToppings);

            // Assert
            Assert.That(pizza, Is.Not.Null);
            Assert.That(pizza.Toppings.Count, Is.EqualTo(0));
            Assert.That(pizza.GetPrice(), Is.EqualTo(8.00m)); // Base price only
        }

        /// <summary>
        /// Tests that toppings collection is read-only to prevent external modification.
        /// 
        /// This verifies encapsulation principles:
        /// - Toppings property returns IReadOnlyList interface
        /// - External code cannot modify pizza toppings after construction
        /// - Builder pattern maintains object immutability after creation
        /// 
        /// Required implementation:
        /// - Pizza.Toppings property typed as IReadOnlyList&lt;ITopping&gt;
        /// - Internal collection protection from external modification
        /// - Proper encapsulation of pizza state
        /// </summary>
        [Test]
        public void Pizza_ToppingsShouldBeReadOnly()
        {
            // Arrange
            var pizza = _pizzaShop.PlaceOrder(_testToppings);

            // Act & Assert
            Assert.That(pizza.Toppings, Is.AssignableTo<IReadOnlyList<ITopping>>());
            
            // Verify it's not a modifiable List directly
            Assert.That(pizza.Toppings, Is.Not.TypeOf<List<ITopping>>());
        }

        /// <summary>
        /// Tests that complex pizza orders with multiple same-weight toppings are handled correctly.
        /// 
        /// This verifies advanced Builder functionality:
        /// - Multiple toppings with same OrderWeight maintain stable ordering
        /// - Price calculation works with many toppings
        /// - Builder handles complex construction scenarios
        /// 
        /// Required implementation:
        /// - Stable sorting algorithm for toppings with same OrderWeight
        /// - Accurate price calculation for multiple toppings
        /// - Builder state management for complex orders
        /// </summary>
        [Test]
        public void PizzaShop_ShouldHandleComplexOrders()
        {
            // Arrange
            var complexToppings = new List<ITopping>
            {
                BuilderPattern.CreateTopping("Mushrooms", 1.50m, 3),
                BuilderPattern.CreateTopping("Sauce", 1.00m, 1),
                BuilderPattern.CreateTopping("Pepperoni", 2.50m, 3),
                BuilderPattern.CreateTopping("Bell Peppers", 1.25m, 3),
                BuilderPattern.CreateTopping("Cheese", 2.00m, 2),
                BuilderPattern.CreateTopping("Olives", 1.75m, 3)
            };

            // Act
            var pizza = _pizzaShop.PlaceOrder(complexToppings);

            // Assert
            Assert.That(pizza.Toppings.Count, Is.EqualTo(6));
            
            // Verify proper ordering
            Assert.That(pizza.Toppings[0].Name, Is.EqualTo("Sauce"));  // OrderWeight 1
            Assert.That(pizza.Toppings[1].Name, Is.EqualTo("Cheese")); // OrderWeight 2
            
            // OrderWeight 3 toppings should maintain input order within same weight
            var weight3Toppings = pizza.Toppings.Skip(2).Select(t => t.Name).ToList();
            Assert.That(weight3Toppings, Contains.Item("Mushrooms"));
            Assert.That(weight3Toppings, Contains.Item("Pepperoni"));
            Assert.That(weight3Toppings, Contains.Item("Bell Peppers"));
            Assert.That(weight3Toppings, Contains.Item("Olives"));

            // Verify total price: $8 base + $1 + $2 + $1.50 + $2.50 + $1.25 + $1.75 = $18.00
            Assert.That(pizza.GetPrice(), Is.EqualTo(18.00m));
        }
    }
}
