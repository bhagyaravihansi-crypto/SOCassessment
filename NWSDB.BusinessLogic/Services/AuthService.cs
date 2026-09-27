using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDB.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly NwsdbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(NwsdbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // REGISTER

        public async Task<(bool Success, string Message)> RegisterAsync(
            RegisterUserDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Email == dto.Email);

            if (existingUser != null)
            {
                return (
                    false,
                    "An account with this email already exists."
                );
            }

            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(
                    c => c.AccountNumber == dto.AccountNumber);

            if (existingCustomer != null)
            {
                return (
                    false,
                    "This account number is already registered."
                );
            }

            var user = new User
            {
                FullName = dto.FullName,
                AccountNumber = dto.AccountNumber,
                NIC = dto.NIC,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);

            _context.Users.Add(user);

            var customer = new Customer
            {
                AccountNumber = dto.AccountNumber,
                FullName = dto.FullName,
                NIC = dto.NIC,
                Email = dto.Email,
                Phone = dto.PhoneNumber,
                Address = dto.Address,
                Status = "Active",
                RegistrationDate = DateTime.UtcNow
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return (
                true,
                "Registration successful."
            );
        }

        // LOGIN

        public async Task<(
            bool Success,
            string Message,
            int UserId,
            string Role)> LoginAsync(LoginDto dto)
        {
            var email = dto.Email.Trim();

            var user = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Email == email);

            // User not found
            if (user == null)
            {
                return (
                    false,
                    "DEBUG 1: User not found. Email = " + email,
                    0,
                    string.Empty
                );
            }

            // PasswordHash missing
            if (string.IsNullOrWhiteSpace(
                user.PasswordHash))
            {
                return (
                    false,
                    "DEBUG 2: User found, but PasswordHash is empty.",
                    0,
                    string.Empty
                );
            }

            // Check password
            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            // Password incorrect
            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                return (
                    false,
                    "DEBUG 3: User found, but password is incorrect.",
                    0,
                    string.Empty
                );
            }

            // Login successful
            return (
                true,
                "Login successful.",
                user.Id,
                user.Role
            );
        }
    }
}