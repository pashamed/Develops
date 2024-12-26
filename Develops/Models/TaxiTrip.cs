namespace Develops.Models
{
    internal class TaxiTrip
    {
        public DateTime PickupDateTime { get; set; }
        public DateTime DropoffDateTime { get; set; }
        public int PassengerCount { get; set; }
        public decimal TripDistance { get; set; }
        public string StoreAndFwdFlag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }

        public bool IsValid(out string validationError)
        {
            validationError = string.Empty;

            if (PickupDateTime > DropoffDateTime)
            {
                validationError = "PickupDateTime cannot be later than DropoffDateTime.";
                return false;
            }

            if (PassengerCount < 0)
            {
                validationError = "PassengerCount cannot be negative.";
                return false;
            }

            if (TripDistance < 0)
            {
                validationError = "TripDistance cannot be negative.";
                return false;
            }

            if (FareAmount < 0)
            {
                validationError = "FareAmount cannot be negative.";
                return false;
            }

            if (TipAmount < 0)
            {
                validationError = "TipAmount cannot be negative.";
                return false;
            }

            return true;
        }
    }
}