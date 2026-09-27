namespace NWSDB.BusinessLogic.DTOs
{
    public class RegisterUserDto
    {
        public string FullName { get; set; } = string.Empty;

        public string? AccountNumber { get; set; }

        public string? NIC { get; set; }

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";
    }
}