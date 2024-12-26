using Develops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Develops.Services
{
    static internal class DuplicateRemovalService
    {
        public static List<TaxiTrip> RemoveDuplicates(List<TaxiTrip> records, string duplicatesFilePath)
        {
            var duplicates = records
                .GroupBy(r => new { r.PickupDateTime, r.DropoffDateTime, r.PassengerCount })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1))
                .ToList();

            using (var writer = new StreamWriter(duplicatesFilePath))
            {
                // Write header
                writer.WriteLine("PickupDateTime,DropoffDateTime,PassengerCount,TripDistance,StoreAndFwdFlag,PULocationID,DOLocationID,FareAmount,TipAmount");

                foreach (var trip in duplicates)
                {
                    writer.WriteLine($"{trip.PickupDateTime:o}," +
                        $"{trip.DropoffDateTime:o}," +
                        $"{trip.PassengerCount},{trip.TripDistance}," +
                        $"{trip.StoreAndFwdFlag},{trip.PULocationID}," +
                        $"{trip.DOLocationID}," +
                        $"{trip.FareAmount}," +
                        $"{trip.TipAmount}");
                }
            }

            return records
                .GroupBy(r => new { r.PickupDateTime, r.DropoffDateTime, r.PassengerCount })
                .Select(g => g.First())
                .ToList();
        }
    }
}
