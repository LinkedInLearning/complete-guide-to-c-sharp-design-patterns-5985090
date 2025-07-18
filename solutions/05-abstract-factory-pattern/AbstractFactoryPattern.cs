/*
Abstract Factory Pattern

Summary:
The Abstract Factory Pattern provides an interface for creating families of related or dependent objects without specifying their concrete classes. It encapsulates a group of individual factories that have a common theme.

Problem to Solve:
In a data access layer that needs to support multiple database providers (SQL Server, MySQL), each provider has different connection string formats and connection behaviors. The challenge is to create database objects without tightly coupling to specific provider implementations while maintaining consistency within each provider family.
*/

using System;
using System.Text;

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

    internal class SqlServerConnection : IDbConnection
    {

        public string ConnectionString { get; }
        public bool IsOpen { get; private set; }

        public SqlServerConnection(string server, string database, string username = "", string password = "")
        {
            var builder = new StringBuilder();
            builder.Append("Server=").Append(server);
            builder.Append(";Database=").Append(database);
            builder.Append(";Integrated Security=true");
            ConnectionString = builder.ToString();
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public string GetProviderName() => "SQL Server";
        public string GetStatus() => IsOpen ? "Open" : "Closed";

    }

    internal class MySqlServerConnection : IDbConnection
    {

        public string ConnectionString { get; }
        public bool IsOpen { get; private set; }

        public MySqlServerConnection(string server, string database, string username = "", string password = "")
        {
            var builder = new StringBuilder();
            builder.Append("Server=").Append(server);
            builder.Append(";Database=").Append(database);
            builder.Append(";Uid=").Append(username);
            builder.Append(";Pwd=").Append(password);
            ConnectionString = builder.ToString();
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public string GetProviderName() => "MySQL";
        public string GetStatus() => IsOpen ? "Open" : "Closed";

    }

    internal class SqlServerConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection(string server, string database, string username = "", string password = "") => new SqlServerConnection(server, database, username, password);
    }

    internal class MySqlServerConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection(string server, string database, string username = "", string password = "") => new MySqlServerConnection(server, database, username, password);
    }

    internal class DatabaseManager : IDbManager
    {

        private readonly IDbConnectionFactory _factory;
        private IDbConnection? _connection;

        public DatabaseManager(IDbConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentException(nameof(factory));
        }

        public void Connect(string server, string database, string username = "", string password = "")
        {
            _connection = _factory.CreateConnection(server, database, username, password);
            _connection.Open();
        }

        public void Disconnect()
        {
            _connection?.Close();
            _connection = null;
        }

        public string GetConnectionInfo()
        {
            if (_connection == null)
                return "Not connected";

            var builder = new StringBuilder();
            builder.Append("Provider: ").Append(_connection.GetProviderName());
            builder.Append("\nConnection String: ").Append(_connection.ConnectionString);
            builder.Append("\nStatus: ").Append(_connection.GetStatus());
            return builder.ToString();
        }

        public bool IsConnected => _connection?.IsOpen ?? false;

    }



    /// <summary>
    /// Creates the abstract factory pattern with database provider families
    /// </summary>
    public static class AbstractFactoryPattern
    {
        public static (IDbConnectionFactory factory, IDbManager manager) Create(string provider = "SqlServer")
        {
            IDbConnectionFactory factory = provider.ToLower() switch
            {
                "mysql" => new MySqlServerConnectionFactory(),
                _ => new SqlServerConnectionFactory()
            };

            return (factory, new DatabaseManager(factory));
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
