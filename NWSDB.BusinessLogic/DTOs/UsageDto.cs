using System;

namespace NWSDB.BusinessLogic.DTOs
{
    public class UsageDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BillingMonth { get; set; } = string.Empty;
        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }
        public int UnitsConsumed { get; set; }
        public DateTime ReadingDate { get; set; }
        public string UsageStatus { get; set; } = string.Empty;
    }
}
