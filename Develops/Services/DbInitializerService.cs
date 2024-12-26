using Develops.Config;
using Microsoft.Data.SqlClient;

namespace Develops.Services
{
    internal static class DbInitializerService
    {
        public static void InitializeDatabase(AppSettings appSettings)
        {
            string masterConnectionString = GetMasterConnectionString(appSettings.DatabaseConnectionString);
            string databaseName = GetDatabaseName(appSettings.DatabaseConnectionString);

            // Check if the database exists
            if (!DatabaseExists(masterConnectionString, databaseName))
            {
                // Create the database
                Console.WriteLine($"Database '{databaseName}' does not exist. Creating...");
                CreateDatabase(masterConnectionString, databaseName);
                Console.WriteLine($"Database '{databaseName}' created successfully.");

                // Execute the SQL script to create schema
                ExecuteSqlScript(appSettings.DatabaseConnectionString, appSettings.SqlScriptFilePath);
                Console.WriteLine("Database schema created successfully.");
            }
            else
            {
                Console.WriteLine($"Database '{databaseName}' already exists.");
            }
        }

        private static string GetMasterConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        private static string GetDatabaseName(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }

        private static bool DatabaseExists(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            connection.Open();

            string checkQuery = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";
            using var command = new SqlCommand(checkQuery, connection);
            var result = command.ExecuteScalar();

            return result != null && result != DBNull.Value;
        }

        private static void CreateDatabase(string masterConnectionString, string databaseName)
        {
            using var connection = new SqlConnection(masterConnectionString);
            connection.Open();

            string createQuery = $"CREATE DATABASE [{databaseName}]";
            using var command = new SqlCommand(createQuery, connection);
            command.ExecuteNonQuery();
        }

        private static void ExecuteSqlScript(string connectionString, string scriptFilePath)
        {
            string script = File.ReadAllText(scriptFilePath);

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            foreach (var commandText in SplitSqlStatements(script))
            {
                using var command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }

        private static IEnumerable<string> SplitSqlStatements(string sqlScript)
        {
            var statements = sqlScript.Split(new[] { "GO", "go", "Go" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var stmt in statements)
            {
                if (!string.IsNullOrWhiteSpace(stmt))
                {
                    yield return stmt.Trim();
                }
            }
        }
    }
}