/*
Builder Pattern

Summary:
The Builder Pattern allows for the step-by-step construction of complex objects using the same construction process to create different representations. It separates the construction of an object from its representation, making it possible to create different types and representations using the same construction code.

Problem to Solve:
In a pizza ordering system, customers need to build custom pizzas with various toppings, sizes, and configurations. The challenge is to construct these complex pizza objects step by step while ensuring proper topping order (sauce first, then cheese, then other toppings) and maintaining a clean, readable interface for the construction process.
*/

using System;
using System.Collections.Generic;

namespace DotNetDesignPatterns.Patterns.Creational
{
    /// <summary>
    /// Represents a pizza topping with name, price, and order weight
    /// </summary>
    public interface ITopping
    {
        string Name { get; }
        decimal Price { get; }
        int OrderWeight { get; }
    }

    /// <summary>
    /// Represents a completed pizza with toppings and price calculation
    /// </summary>
    public interface IPizza
    {
        IReadOnlyList<ITopping> Toppings { get; }
        decimal GetPrice();
    }

    /// <summary>
    /// Interface for pizza shop that processes orders using Builder pattern
    /// </summary>
    public interface IPizzaShop
    {
        IPizza PlaceOrder(IList<ITopping> toppings);
    }

    public class Topping : ITopping
    {
        public string Name { get; }
        public decimal Price { get; }
        public int OrderWeight { get; }

        public Topping(string name, decimal price, int orderWeight = 3)
        {
            Name = name;
            Price = price;
            OrderWeight = orderWeight;
        }
    }

    public class Pizza : IPizza
    {
        public IReadOnlyList<ITopping> Toppings { get; }

        public Pizza(IReadOnlyList<ITopping> toppings)
        {
            Toppings = toppings.OrderBy(t => t.OrderWeight).ToArray();
        }

        public decimal GetPrice()
        {
            decimal basePrice = 8.00m;
            return basePrice + Toppings.Sum(t => t.Price);
        }
    }

    public class PizzaShop : IPizzaShop
    {
        private class PizzaBuilder
        {
            private readonly List<ITopping> _toppings = new();

            public PizzaBuilder AddTopping(ITopping topping)
            {
                _toppings.Add(topping);
                return this;
            }

            public IPizza Build()
            {
                var pizza = new Pizza(_toppings.ToList());
                _toppings.Clear();
                return pizza;
            }
        }

        public IPizza PlaceOrder(IList<ITopping> toppings)
        {
            var builder = new PizzaBuilder();

            foreach (var topping in toppings)
            {
                builder.AddTopping(topping);
            }

            return builder.Build();
        }
    }

    /// <summary>
    /// Creates a pizza shop ordering system
    /// </summary>
    public static class BuilderPattern
    {
        public static IPizzaShop Create()
        {
            return new PizzaShop();
        }

        public static ITopping CreateTopping(string name, decimal price, int orderWeight)
        {
            return new Topping(name, price, orderWeight);
        }

        public static IList<ITopping> CreateTestToppings()
        {
            return new List<ITopping>
            {
                CreateTopping("Sauce", 1.00m, 1),
                CreateTopping("Cheese", 2.00m, 2),
                CreateTopping("Pepperoni", 2.50m, 3),
                CreateTopping("Mushrooms", 1.50m, 3)
            };
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Topping class implementing ITopping (already provided):
       - Constructor takes name, price, and orderWeight parameters
       - OrderWeight determines layering: Sauce (1), Cheese (2), Others (3+)

    2. Pizza class implementing IPizza:
       - Store toppings list sorted by OrderWeight for proper layering
       - GetPrice() calculates base pizza ($8.00) + sum of all topping prices
       - Toppings property returns read-only list in correct order

    3. PizzaShop class implementing IPizzaShop:
       - Contains private PizzaBuilder class for step-by-step construction
       - PlaceOrder(toppings) uses internal builder to construct pizza
       - Builder ensures toppings are properly ordered regardless of input sequence

    4. Internal PizzaBuilder class (private to PizzaShop):
       - AddTopping() method returns builder instance for fluent chaining
       - Build() method sorts toppings by OrderWeight and creates final Pizza
       - Resets builder state after creating each pizza

    5. Factory methods:
       - Create() returns working PizzaShop instance
       - CreateTopping() creates individual topping instances
       - CreateTestToppings() creates standard collection for testing

    Sample toppings with OrderWeight:
    - Sauce: $1.00, OrderWeight 1 (goes first)
    - Cheese: $2.00, OrderWeight 2 (goes second)  
    - Pepperoni: $2.50, OrderWeight 3 (goes third)
    - Mushrooms: $1.50, OrderWeight 3 (goes fourth)

    The tests will verify that:
    - Pizza prices are calculated correctly (base + toppings)
    - Toppings are properly ordered by OrderWeight regardless of input order
    - Builder pattern encapsulates complex construction logic
    - Different pizza configurations can be built using the same process
    - Factory methods create all required objects
    */
}
