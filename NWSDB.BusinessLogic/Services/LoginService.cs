using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDB.BusinessLogic.Services
{
    public class LoginService : ILoginService
    {
        private readonly NwsdbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginService(NwsdbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<(bool Success, string Message, string? Role, int UserId)> LoginAsync(
            LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null, 0);
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return (false, "Invalid email or password.", null, 0);
            }

            return (true, "Login successful.", user.Role, user.Id);
        }
    }
}