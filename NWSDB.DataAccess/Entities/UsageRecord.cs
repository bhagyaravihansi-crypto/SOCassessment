using System;

namespace NWSDB.DataAccess.Entities
{
    public class UsageRecord
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string BillingMonth { get; set; } = string.Empty; // e.g. Aug 2026
        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }
        public int UnitsConsumed { get; set; }
        public DateTime ReadingDate { get; set; } = DateTime.UtcNow;
        public string UsageStatus { get; set; } = "Normal"; // Normal/Moderate/High

    }
}
