using NUnit.Framework;
using System;
using System.Linq;

using DotNetDesignPatterns.Patterns.Creational;


namespace DotNetDesignPatterns.Tests.Patterns.Creational
{
    /// <summary>
    /// Tests for the Factory Method Pattern implementation.
    /// 
    /// The Factory Method pattern defines an interface for creating objects, but lets subclasses
    /// decide which class to instantiate. It delegates object creation to subclasses, promoting
    /// loose coupling by eliminating the need to bind application-specific classes into the code.
    /// 
    /// Required implementation components:
    /// 1. Product interface (IShape) that defines the common interface for created objects
    /// 2. Concrete product classes (Circle, Rectangle, Triangle) that implement IShape
    /// 3. Factory interface (IShapeFactory) that declares the factory method
    /// 4. Concrete factory class that implements the creation logic
    /// 5. Factory method that returns products based on type parameters
    /// 
    /// These tests verify the Factory Method implementation works correctly with proper
    /// object creation, type handling, and interface compliance.
    /// </summary>
    [TestFixture]
    public class FactoryMethodPatternTests
    {
        private IShapeFactory _shapeFactory = null!;

        [SetUp]
        public void Setup()
        {
            // Use the factory method to create test components
            _shapeFactory = FactoryMethodPattern.Create();
        }

        #region Factory Interface Tests

        [Test]
        public void Factory_ShouldImplementIShapeFactoryInterface()
        {
            // Assert
            Assert.That(_shapeFactory, Is.InstanceOf<IShapeFactory>(),
                "Factory should implement IShapeFactory interface");
        }

        [Test]
        public void Factory_ShouldHaveCreateShapeMethod()
        {
            // This test verifies the factory method signature exists
            var factoryType = _shapeFactory.GetType();
            var method = factoryType.GetMethod("CreateShape", new[] { typeof(Point[]) });
            
            Assert.That(method, Is.Not.Null, "Factory should have CreateShape method that takes Point[]");
        }

        #endregion

        #region Shape Creation Tests

        [Test]
        public void CreateShape_ShouldCreateCorrectShapeByPointCount()
        {
            // Test with 2 points (Circle)
            var circle = _shapeFactory.CreateShape(
                new Point(0, 0), 
                new Point(5, 0)
            );
            
            Assert.That(circle, Is.Not.Null, "Factory should create a shape with 2 points");
            Assert.That(circle.GetType().Name.ToLower(), Contains.Substring("circle"), 
                "Factory should create Circle when given 2 points");
            Assert.That(circle.GetShapePoints().Count, Is.EqualTo(2), 
                "Circle should return exactly 2 points");
            
            // Test with 3 points (Triangle)
            var triangle = _shapeFactory.CreateShape(
                new Point(0, 0), 
                new Point(10, 0),
                new Point(5, 8)
            );
            
            Assert.That(triangle, Is.Not.Null, "Factory should create a shape with 3 points");
            Assert.That(triangle.GetType().Name.ToLower(), Contains.Substring("triangle"), 
                "Factory should create Triangle when given 3 points");
            Assert.That(triangle.GetShapePoints().Count, Is.EqualTo(3), 
                "Triangle should return exactly 3 points");
            
            // Test with 4 points (Square)
            var square = _shapeFactory.CreateShape(
                new Point(0, 0),
                new Point(10, 0),
                new Point(10, 10),
                new Point(0, 10)
            );
            
            Assert.That(square, Is.Not.Null, "Factory should create a shape with 4 points");
            Assert.That(square.GetType().Name.ToLower(), Contains.Substring("square"), 
                "Factory should create Square when given 4 points");
            Assert.That(square.GetShapePoints().Count, Is.EqualTo(4), 
                "Square should return exactly 4 points");
        }
        
        [Test]
        public void CreateShape_ShouldThrowExceptionForInvalidPointCount()
        {
            // No points
            Assert.Throws<ArgumentException>(() => _shapeFactory.CreateShape());
            
            // 1 point - not enough for any shape
            Assert.Throws<ArgumentException>(() => _shapeFactory.CreateShape(new Point(0, 0)));
            
            // 5+ points - too many for any shape
            Assert.Throws<ArgumentException>(() => _shapeFactory.CreateShape(
                new Point(0, 0), new Point(1, 1), new Point(2, 2), 
                new Point(3, 3), new Point(4, 4)
            ));
        }
        
        [Test]
        public void CreateShape_ShouldCreateShapesWithCorrectGeometry()
        {
            // Create a Circle with two points
            var circle = _shapeFactory.CreateShape(new Point(0, 0), new Point(5, 0));
            var circlePoints = circle.GetShapePoints();
            
            // Create a Triangle with three points
            var triangle = _shapeFactory.CreateShape(
                new Point(0, 0), 
                new Point(10, 0),
                new Point(5, 8)
            );
            var trianglePoints = triangle.GetShapePoints();
            
            // Create a Square with four points
            var square = _shapeFactory.CreateShape(
                new Point(0, 0),
                new Point(10, 0),
                new Point(10, 10),
                new Point(0, 10)
            );
            var squarePoints = square.GetShapePoints();
            
            // Verify points are returned correctly
            Assert.That(circlePoints.Count, Is.EqualTo(2), "Circle should return 2 points");
            Assert.That(trianglePoints.Count, Is.EqualTo(3), "Triangle should return 3 points");
            Assert.That(squarePoints.Count, Is.EqualTo(4), "Square should return 4 points");
        }

        #endregion

        #region Specific Shape Tests

        [Test]
        public void Factory_ShouldCreateCircleWithTwoPoints()
        {
            // Create Circle with two points
            var circle = _shapeFactory.CreateShape(new Point(0, 0), new Point(5, 0));
            
            // Verify it's a Circle
            Assert.That(circle.GetType().Name.ToLower(), Contains.Substring("circle"));
            
            // Verify it has exactly 2 points
            var points = circle.GetShapePoints();
            Assert.That(points.Count, Is.EqualTo(2), 
                "Circle should return exactly 2 points (center and radius point)");
        }

        [Test]
        public void Factory_ShouldCreateSquareWithFourPoints()
        {
            // Create Square with four points
            var square = _shapeFactory.CreateShape(
                new Point(0, 0), new Point(10, 0), 
                new Point(10, 10), new Point(0, 10)
            );
            
            // Verify it's a Square
            Assert.That(square.GetType().Name.ToLower(), Contains.Substring("square"));
            
            // Verify it has exactly 4 points
            var points = square.GetShapePoints();
            Assert.That(points.Count, Is.EqualTo(4), 
                "Square should return exactly 4 points (corners)");
        }

        [Test]
        public void Factory_ShouldCreateTriangleWithThreePoints()
        {
            // Create Triangle with three points
            var triangle = _shapeFactory.CreateShape(
                new Point(0, 0), new Point(10, 0), new Point(5, 8)
            );
            
            // Verify it's a Triangle
            Assert.That(triangle.GetType().Name.ToLower(), Contains.Substring("triangle"));
            
            // Verify it has exactly 3 points
            var points = triangle.GetShapePoints();
            Assert.That(points.Count, Is.EqualTo(3), 
                "Triangle should return exactly 3 points (vertices)");
        }

        #endregion


    }
}
