/*
Abstract Factory Pattern

Summary:
The Abstract Factory Pattern provides an interface for creating families of related or dependent objects without specifying their concrete classes. It encapsulates a group of individual factories that have a common theme.

Problem to Solve:
In a data access layer that needs to support multiple database providers (SQL Server, MySQL), each provider has different connection string formats and connection behaviors. The challenge is to create database objects without tightly coupling to specific provider implementations while maintaining consistency within each provider family.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Creational
{
    /// <summary>
    /// Abstract product interface representing database connections with provider-specific implementations.
    /// </summary>
    public interface IDbConnection
    {
        string ConnectionString { get; }
        void Open();
        void Close();
        bool IsOpen { get; }
        string GetProviderName();
        string GetStatus();
    }

    /// <summary>
    /// Abstract factory interface for creating database connections, allowing different providers to implement their own creation logic.
    /// </summary>
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string server, string database, string username = "", string password = "");
    }

    /// <summary>
    /// Client interface that uses the abstract factory to manage database connections without knowing the specific provider.
    /// </summary>
    public interface IDbManager
    {
        void Connect(string server, string database, string username = "", string password = "");
        void Disconnect();
        string GetConnectionInfo();
        bool IsConnected { get; }
    }

    /// <summary>
    /// Creates the abstract factory pattern with database provider families
    /// </summary>
    public static class AbstractFactoryPattern
    {
        public static (IDbConnectionFactory factory, IDbManager manager) Create(string provider = "SqlServer")
        {
            throw new NotImplementedException("Implement the abstract factory pattern for database providers");
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Concrete connection classes for each database provider:
       - SqlServerConnection: ConnectionString format "Server={server};Database={database};Integrated Security=true"
       - MySqlConnection: ConnectionString format "Server={server};Database={database};Uid={username};Pwd={password}"
       - Both should track Open/Closed state and return appropriate provider names

    2. Concrete factory classes implementing IDbConnectionFactory:
       - SqlServerConnectionFactory: Creates SQL Server connections
       - MySqlConnectionFactory: Creates MySQL connections
       - Each factory creates connections with provider-specific formatting

    3. DbManager class implementing IDbManager:
       - Takes a connection factory in constructor
       - Connect() creates and opens a connection using the factory
       - GetConnectionInfo() returns provider name and connection details
       - Manages connection lifecycle (connect/disconnect)

    4. Update Create method to:
       - Return appropriate factory and manager based on provider parameter
       - Support "SqlServer" and "MySQL" provider types
       - Wire up the factory and manager correctly

    The tests will verify that:
    - Different providers create connections with correct connection string formats
    - Factories create provider-specific connection objects
    - Database manager works with any factory implementation
    - Provider families maintain consistency in their implementations
    */
}
