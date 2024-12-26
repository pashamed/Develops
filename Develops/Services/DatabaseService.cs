using Develops.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Develops.Services
{
    internal class DatabaseService
    {
        private readonly string connectionString;

        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void BulkInsertRecords(List<TaxiTrip> records)
        {
            // Convert records to DataTable
            var table = new DataTable();
            table.Columns.Add("PickupDateTime", typeof(DateTime));
            table.Columns.Add("DropoffDateTime", typeof(DateTime));
            table.Columns.Add("PassengerCount", typeof(int));
            table.Columns.Add("TripDistance", typeof(decimal));
            table.Columns.Add("StoreAndFwdFlag", typeof(string));
            table.Columns.Add("PULocationID", typeof(int));
            table.Columns.Add("DOLocationID", typeof(int));
            table.Columns.Add("FareAmount", typeof(decimal));
            table.Columns.Add("TipAmount", typeof(decimal));

            foreach (var trip in records)
            {
                table.Rows.Add(
                    trip.PickupDateTime,
                    trip.DropoffDateTime,
                    trip.PassengerCount,
                    trip.TripDistance,
                    trip.StoreAndFwdFlag,
                    trip.PULocationID,
                    trip.DOLocationID,
                    trip.FareAmount,
                    trip.TipAmount
                );
            }

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            using var sqlTransaction = sqlConnection.BeginTransaction();
            using var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction)
            {
                DestinationTableName = "TaxiTrip",
            };

            // Map columns
            bulkCopy.ColumnMappings.Add("PickupDateTime", "PickupDateTime");
            bulkCopy.ColumnMappings.Add("DropoffDateTime", "DropoffDateTime");
            bulkCopy.ColumnMappings.Add("PassengerCount", "PassengerCount");
            bulkCopy.ColumnMappings.Add("TripDistance", "TripDistance");
            bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "StoreAndFwdFlag");
            bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
            bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
            bulkCopy.ColumnMappings.Add("FareAmount", "FareAmount");
            bulkCopy.ColumnMappings.Add("TipAmount", "TipAmount");

            try
            {
                bulkCopy.WriteToServer(table);
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                Console.WriteLine($"Error during bulk insert: {ex.Message}");
            }
        }

        public int GetRecordCount()
        {
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using var sqlCommand = new SqlCommand("SELECT COUNT(*) FROM TaxiTrip", sqlConnection);
            return (int)sqlCommand.ExecuteScalar();
        }
    }
}