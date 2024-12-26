// See https://aka.ms/new-console-template for more information
using Develops.Config;
using Develops.Services;
using Microsoft.Extensions.Configuration;

namespace Develops
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration configuration = LoadConfiguration();
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings == null)
            {
                Console.WriteLine("AppSettings section is missing or invalid in the configuration file.");
                return;
            }
            else if(!File.Exists(appSettings.CsvFilePath))
            {
                Console.WriteLine("Provide correct CSV import file path");
                return;
            }

            var csvService = new CsvService();
            var databaseService = new DatabaseService(appSettings.DatabaseConnectionString);

            DbInitializerService.InitializeDatabase(appSettings);

            Console.WriteLine("Reading and processing CSV file...");
            var records = csvService.ReadAndProcessCsv(appSettings);

            Console.WriteLine("Removing duplicates...");
            var uniqueRecords = DuplicateRemovalService.RemoveDuplicates(records, appSettings.DuplicatesFilePath);

            Console.WriteLine("Inserting data into the database...");
            databaseService.BulkInsertRecords(uniqueRecords);

            Console.WriteLine("ETL process completed successfully.");
            var totalRowsInserted = databaseService.GetRecordCount();
            Console.WriteLine($"Total rows inserted: {totalRowsInserted}");
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}