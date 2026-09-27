using System;

namespace NWSDB.DataAccess.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentReference { get; set; } = string.Empty; // e.g. PAY-0001

        // Relationship
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? BillId { get; set; }
        public Bill? Bill { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Card/Mobile/Bank
        public string Status { get; set; } = "Successful"; // Successful/Pending/Failed
    }
}
