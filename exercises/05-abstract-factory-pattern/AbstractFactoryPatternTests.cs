using System;
using NUnit.Framework;
using DotNetDesignPatterns.Patterns.Creational;

namespace DotNetDesignPatterns.Tests.Patterns.Creational
{
    /// <summary>
    /// Tests for the Abstract Factory Pattern implementation.
    /// 
    /// The Abstract Factory pattern provides an interface for creating families of related
    /// or dependent objects without specifying their concrete classes. It encapsulates
    /// object creation logic and ensures that created objects are compatible with each other.
    /// 
    /// Required implementation components:
    /// 1. Abstract factory interface (IDbConnectionFactory) that declares creation methods
    /// 2. Concrete factories (SqlServerFactory, MySqlFactory) for specific product families
    /// 3. Abstract product interfaces (IDbConnection, IDbManager) that define product contracts
    /// 4. Concrete products (SqlServerConnection, MySqlConnection) that implement specific behaviors
    /// 5. Factory method to create appropriate factory based on provider type
    /// 
    /// This implementation demonstrates creating database connections and managers for
    /// different database providers (SQL Server, MySQL) while maintaining consistent interfaces.
    /// The pattern ensures that related objects (connections and managers) are compatible.
    /// </summary>
    [TestFixture]
    public class AbstractFactoryPatternTests
    {
        /// <summary>
        /// Factory instances for creating SQL Server-specific database objects.
        /// Should create connections and managers optimized for SQL Server.
        /// </summary>
        private IDbConnectionFactory _sqlServerFactory = null!;
        
        /// <summary>
        /// Factory instances for creating MySQL-specific database objects.
        /// Should create connections and managers optimized for MySQL.
        /// </summary>
        private IDbConnectionFactory _mySqlFactory = null!;
        
        /// <summary>
        /// Database manager for SQL Server operations.
        /// Should work specifically with SQL Server connections and provide SQL Server-specific functionality.
        /// </summary>
        private IDbManager _sqlServerManager = null!;
        
        /// <summary>
        /// Database manager for MySQL operations.
        /// Should work specifically with MySQL connections and provide MySQL-specific functionality.
        /// </summary>
        private IDbManager _mySqlManager = null!;

        /// <summary>
        /// Setup method that creates factory instances for different database providers.
        /// 
        /// Required implementation:
        /// - AbstractFactoryPattern.Create(string) method that accepts provider type
        /// - Method should return appropriate factory and manager for the specified provider
        /// - Support for "SqlServer" and "MySQL" provider types
        /// - Each factory should create provider-specific objects
        /// </summary>
        [SetUp]
        public void Setup()
        {
            (_sqlServerFactory, _sqlServerManager) = AbstractFactoryPattern.Create("SqlServer");
            (_mySqlFactory, _mySqlManager) = AbstractFactoryPattern.Create("MySQL");
        }

        [Test]
        public void SqlServerFactory_ShouldCreateSqlServerConnection()
        {
            // Act
            var connection = _sqlServerFactory.CreateConnection("localhost", "TestDB");

            // Assert
            Assert.That(connection.GetProviderName(), Is.EqualTo("SQL Server"));
            Assert.That(connection.ConnectionString, Does.Contain("Server=localhost"));
            Assert.That(connection.ConnectionString, Does.Contain("Database=TestDB"));
            Assert.That(connection.ConnectionString, Does.Contain("Integrated Security=true"));
        }

        [Test]
        public void MySqlFactory_ShouldCreateMySqlConnection()
        {
            // Act
            var connection = _mySqlFactory.CreateConnection("localhost", "TestDB", "user", "pass");

            // Assert
            Assert.That(connection.GetProviderName(), Is.EqualTo("MySQL"));
            Assert.That(connection.ConnectionString, Does.Contain("Server=localhost"));
            Assert.That(connection.ConnectionString, Does.Contain("Database=TestDB"));
            Assert.That(connection.ConnectionString, Does.Contain("Uid=user"));
            Assert.That(connection.ConnectionString, Does.Contain("Pwd=pass"));
        }

        [Test]
        public void Manager_ShouldWorkWithDifferentProviders()
        {
            // Act
            _sqlServerManager.Connect("localhost", "TestDB");
            _mySqlManager.Connect("localhost", "TestDB", "user", "pass");

            // Assert
            Assert.That(_sqlServerManager.GetConnectionInfo(), Does.Contain("SQL Server"));
            Assert.That(_mySqlManager.GetConnectionInfo(), Does.Contain("MySQL"));
        }

        [Test]
        public void DefaultProvider_ShouldBeSqlServer()
        {
            // Act
            var (factory, manager) = AbstractFactoryPattern.Create();
            var connection = factory.CreateConnection("localhost", "TestDB");

            // Assert
            Assert.That(connection.GetProviderName(), Is.EqualTo("SQL Server"));
        }

        [Test]
        public void Connection_WhenOpened_ShouldBeOpen()
        {
            // Arrange
            var connection = _sqlServerFactory.CreateConnection("localhost", "TestDB");

            // Act
            connection.Open();

            // Assert
            Assert.That(connection.IsOpen, Is.True);
            Assert.That(connection.GetStatus(), Is.EqualTo("Open"));
        }

        [Test]
        public void Connection_WhenClosed_ShouldBeClosed()
        {
            // Arrange
            var connection = _sqlServerFactory.CreateConnection("localhost", "TestDB");
            connection.Open();

            // Act
            connection.Close();

            // Assert
            Assert.That(connection.IsOpen, Is.False);
            Assert.That(connection.GetStatus(), Is.EqualTo("Closed"));
        }

        [Test]
        public void Manager_WhenConnected_ShouldShowConnectionInfo()
        {
            // Act
            _sqlServerManager.Connect("localhost", "TestDB");

            // Assert
            Assert.That(_sqlServerManager.IsConnected, Is.True);
            Assert.That(_sqlServerManager.GetConnectionInfo(), Does.Contain("SQL Server"));
            Assert.That(_sqlServerManager.GetConnectionInfo(), Does.Contain("localhost"));
        }

        [Test]
        public void Manager_WhenDisconnected_ShouldNotBeConnected()
        {
            // Arrange
            _sqlServerManager.Connect("localhost", "TestDB");

            // Act
            _sqlServerManager.Disconnect();

            // Assert
            Assert.That(_sqlServerManager.IsConnected, Is.False);
        }
    }
}

