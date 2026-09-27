using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDB.BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly NwsdbContext _db;

        public CustomerService(NwsdbContext db)
        {
            _db = db;
        }

        public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto)
        {
            var customer = new Customer
            {
                AccountNumber = dto.AccountNumber,
                FullName = dto.FullName,
                NIC = dto.NIC,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };

            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();

            return Map(customer);
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var list = await _db.Customers
                .AsNoTracking()
                .ToListAsync();

            return list.Select(Map).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _db.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                return null;

            return Map(customer);
        }

        // Get customer using the logged-in User ID
        public async Task<CustomerDto?> GetByUserIdAsync(int userId)
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            Customer? customer = null;

            // First try Account Number
            if (!string.IsNullOrWhiteSpace(user.AccountNumber))
            {
                customer = await _db.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.AccountNumber == user.AccountNumber);
            }

            // If Account Number does not find a customer,
            // try Email
            if (customer == null && !string.IsNullOrWhiteSpace(user.Email))
            {
                customer = await _db.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.Email == user.Email);
            }

            if (customer == null)
                return null;

            return Map(customer);
        }

        public async Task<bool> UpdateAsync(int id, CustomerUpdateDto dto)
        {
            var customer = await _db.Customers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                return false;

            if (!string.IsNullOrEmpty(dto.FullName))
                customer.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.NIC))
                customer.NIC = dto.NIC;

            if (!string.IsNullOrEmpty(dto.Email))
                customer.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Phone))
                customer.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Address))
                customer.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.Status))
                customer.Status = dto.Status;

            await _db.SaveChangesAsync();

            return true;
        }

        private static CustomerDto Map(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                AccountNumber = customer.AccountNumber,
                FullName = customer.FullName,
                NIC = customer.NIC,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                Status = customer.Status,
                RegistrationDate = customer.RegistrationDate
            };
        }
    }
}