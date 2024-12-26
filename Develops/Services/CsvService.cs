using Develops.Config;
using Develops.Models;
using Develops.Utils;
using System.Globalization;

namespace Develops.Services
{
    internal class CsvService
    {
        public List<TaxiTrip> ReadAndProcessCsv(AppSettings appSettings)
        {
            var records = new List<TaxiTrip>();
            var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appSettings.TimeZone);

            using var reader = new StreamReader(appSettings.CsvFilePath);
            string? headerLine = reader.ReadLine();
            if (string.IsNullOrEmpty(headerLine))
                throw new InvalidDataException("CSV file is empty.");

            var headers = headerLine.Split(',');

            // Get indexes of required columns
            var requiredColumns = new Dictionary<string, int>
                    {
                        { "tpep_pickup_datetime", -1 },
                        { "tpep_dropoff_datetime", -1 },
                        { "passenger_count", -1 },
                        { "trip_distance", -1 },
                        { "store_and_fwd_flag", -1 },
                        { "PULocationID", -1 },
                        { "DOLocationID", -1 },
                        { "fare_amount", -1 },
                        { "tip_amount", -1 }
                    };

            for (int i = 0; i < headers.Length; i++)
            {
                var header = headers[i].Trim();
                if (requiredColumns.ContainsKey(header))
                {
                    requiredColumns[header] = i;
                }
            }

            // Ensure all required columns are present
            if (requiredColumns.Values.Any(index => index == -1))
            {
                throw new InvalidDataException("CSV file does not contain all required columns.");
            }

            string? line;
            int lineNumber = 1;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                var fields = line.Split(',');

                try
                {
                    var trip = new TaxiTrip
                    {
                        PickupDateTime = DateTime.Parse(fields[requiredColumns["tpep_pickup_datetime"]], CultureInfo.InvariantCulture),
                        DropoffDateTime = DateTime.Parse(fields[requiredColumns["tpep_dropoff_datetime"]], CultureInfo.InvariantCulture),
                        PassengerCount = int.Parse(fields[requiredColumns["passenger_count"]]),
                        TripDistance = decimal.Parse(fields[requiredColumns["trip_distance"]], CultureInfo.InvariantCulture),
                        StoreAndFwdFlag = fields[requiredColumns["store_and_fwd_flag"]].Sanitize(),
                        PULocationID = int.Parse(fields[requiredColumns["PULocationID"]]),
                        DOLocationID = int.Parse(fields[requiredColumns["DOLocationID"]]),
                        FareAmount = decimal.Parse(fields[requiredColumns["fare_amount"]], CultureInfo.InvariantCulture),
                        TipAmount = decimal.Parse(fields[requiredColumns["tip_amount"]], CultureInfo.InvariantCulture)
                    };

                    trip.StoreAndFwdFlag = trip.StoreAndFwdFlag == "Y" ? "Yes" : "No";

                    trip.PickupDateTime = TimeZoneInfo.ConvertTimeToUtc(trip.PickupDateTime, estTimeZone);
                    trip.DropoffDateTime = TimeZoneInfo.ConvertTimeToUtc(trip.DropoffDateTime, estTimeZone);
                    records.Add(trip);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine($"Processed {lineNumber-1} records");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing line {lineNumber}: {ex.Message}");
                }
            }

            return records;
        }
    }
}