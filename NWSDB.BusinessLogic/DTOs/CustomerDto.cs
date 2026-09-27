using System;

namespace NWSDB.BusinessLogic.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? NIC { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }
}
