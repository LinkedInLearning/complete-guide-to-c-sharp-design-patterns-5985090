/*
Composite Pattern

Summary:
The Composite Pattern composes objects into tree structures to represent part-whole hierarchies. It lets clients treat individual objects and compositions of objects uniformly, allowing you to build complex tree structures where both individual items and groups of items can be treated the same way.

Problem to Solve:
In organizational management systems, you need to represent company hierarchies with employees, departments, and sub-departments. The challenge is to create a structure where you can perform operations (like getting details or calculating costs) uniformly on both individual employees and entire departments, regardless of the complexity of the organizational tree.
*/

using System;
using System.Collections.Generic;

namespace DotNetDesignPatterns.Patterns.Structural
{
    /// <summary>
    /// Common interface for all organization components, enabling uniform treatment of individuals and groups
    /// </summary>
    public interface IOrgComponent
    {
        string GetDetails();
    }

    /// <summary>
    /// Creates an organization chart with departments and employees
    /// </summary>
    public static class CompositePattern
    {
        public static IOrgComponent CreateOrganization()
        {
            throw new NotImplementedException("Implement organization chart creation");
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Leaf component (Employee):
       - Employee class implementing IOrgComponent for individual workers
       - Constructor takes name and position parameters
       - GetDetails() returns formatted string with employee information
       - Represents the individual objects in the composite structure

    2. Composite component (Department):
       - Department class implementing IOrgComponent for groups of components
       - Constructor takes department name parameter
       - AddComponent() method to build hierarchical structure
       - GetDetails() returns department info plus all contained components
       - Can contain both employees and other departments

    3. Organization structure in CreateOrganization():
       - Create a Company department as root
       - Add Development department containing Alice (Developer) and Bob (Tester)
       - Add Charlie (CEO) directly under the company
       - Return the complete organizational hierarchy

    The tests will verify that:
    - Individual employees and departments implement the same interface
    - Composite structure can be traversed uniformly
    - GetDetails() operation works on both leaves and composites
    - Complex hierarchies can be built and queried consistently
    */
}
