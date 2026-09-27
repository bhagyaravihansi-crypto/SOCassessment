using System;

namespace NWSDB.DataAccess.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public string BillNumber { get; set; } = string.Empty; // e.g. BILL-2026-0001

        // Relationship
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string BillingPeriod { get; set; } = string.Empty; // e.g. Aug 2026
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

        // Meter readings
        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }
        public int UnitsConsumed { get; set; }

        // Charges
        public decimal UsageCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; // Pending/Paid/Overdue
    }
}
