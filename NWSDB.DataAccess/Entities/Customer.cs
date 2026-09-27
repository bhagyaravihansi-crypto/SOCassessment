using System;
using System.Collections.Generic;

namespace NWSDB.DataAccess.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty; // e.g. AC-12345678
        public string FullName { get; set; } = string.Empty;
        public string? NIC { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; } = "Active"; // Active/Inactive
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>();
    }
}
