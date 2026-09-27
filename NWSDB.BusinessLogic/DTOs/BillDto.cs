using System;

namespace NWSDB.BusinessLogic.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BillingPeriod { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }
        public int UnitsConsumed { get; set; }
        public decimal UsageCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
