/*
Factory Method Pattern

Summary:
The Factory Method Pattern defines an interface for creating objects, but lets subclasses decide which classes to instantiate. It delegates the object creation to subclasses and promotes loose coupling by eliminating the need to bind application-specific classes into the code.

Problem to Solve:
In a graphics application, different shapes need to be created based on the number of points provided by users. The challenge is to create the appropriate shape object (Circle, Triangle, Square) without the client code knowing the specific shape classes, while maintaining flexibility to add new shapes and intelligent selection based on input parameters.
*/

using System;
using System.Collections.Generic;

namespace DotNetDesignPatterns.Patterns.Creational
{
    /// <summary>
    /// Point structure for shape definitions
    /// </summary>
    public struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Common interface for all shapes
    /// </summary>
    public interface IShape
    {
        IReadOnlyList<Point> GetShapePoints();
    }

    public class Circle : IShape
    {

        private readonly Point _center;
        private readonly double _radius;

        public Circle(Point center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        public IReadOnlyList<Point> GetShapePoints()
        {

            return new List<Point>
            {
                _center,
                new Point(_center.X + _radius, _center.Y)
            };

        }

    }

    public class Square : IShape
    {

        private readonly Point _topLeft;
        private readonly double _sideLength;

        public Square(Point topLeft, double sideLength)
        {
            _topLeft = topLeft;
            _sideLength = sideLength;
        }

        public IReadOnlyList<Point> GetShapePoints()
        {

            return new List<Point>
            {
                _topLeft,
                new Point(_topLeft.X + _sideLength, _topLeft.Y),
                new Point(_topLeft.X + _sideLength, _topLeft.Y + _sideLength),
                new Point(_topLeft.X, _topLeft.Y + _sideLength)
            };

        }

    }

    public class Triangle : IShape
    {

        private readonly Point _p1;
        private readonly Point _p2;
        private readonly Point _p3;

        public Triangle(Point p1, Point p2, Point p3)
        {
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
        }

        public IReadOnlyList<Point> GetShapePoints()
        {
            return new List<Point>
            {
                _p1,
                _p2,
                _p3
            };
        }

    }


    /// <summary>
    /// Factory interface for creating shapes based on points
    /// </summary>
    public interface IShapeFactory
    {
        IShape CreateShape(params Point[] points);
    }

    public class ShapeFactory : IShapeFactory
    {
        public IShape CreateShape(params Point[] points)
        {
            return points.Length switch
            {
                2 => new Circle(points[0], 5.0),
                3 => new Triangle(points[0], points[1], points[2]),
                4 => new Square(points[0], 10.0),
                _ => throw new ArgumentException($"No shape can be created with {points.Length} points")
            };
        }
    }


    /// <summary>
    /// Creates a shape factory that selects shape types based on point count
    /// </summary>
    public static class FactoryMethodPattern
    {
        public static IShapeFactory Create()
        {
            return new ShapeFactory();
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Circle class implementing IShape:
       - Takes center point and radius in constructor (calculated from two points)
       - GetShapePoints() returns exactly 2 points (center and radius point)
       - First point is center, distance to second point determines radius

    2. Triangle class implementing IShape:
       - Takes three points in constructor for the three vertices
       - GetShapePoints() returns exactly 3 points (the three vertices)
       - Stores and returns the exact points provided

    3. Square class implementing IShape:
       - Takes four corner points in constructor
       - GetShapePoints() returns exactly 4 points (the four corners)
       - Maintains the corner positions as provided

    4. ShapeFactory class implementing IShapeFactory:
       - CreateShape(params Point[] points) selects shape based on point count
       - 2 points → Circle (first point as center, distance to second as radius)
       - 3 points → Triangle (three vertices)
       - 4 points → Square (four corners)
       - Throws ArgumentException for unsupported point counts

    The tests will verify that:
    - Factory creates correct shape types based on point count
    - Each shape returns the expected number of points
    - Circle geometry is calculated correctly from center and radius points
    - Shape creation is decoupled from client code through factory abstraction
    */
}
