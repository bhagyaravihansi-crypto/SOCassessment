using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.DataAccess;
using NWSDB.DataAccess.Entities;

namespace NWSDB.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly NwsdbContext _context;

        public PaymentService(NwsdbContext context)
        {
            _context = context;
        }

        // Get all payments
        public async Task<List<PaymentDto>> GetAllAsync()
        {
            var payments = await _context.Payments
                .AsNoTracking()
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                PaymentReference = p.PaymentReference,
                CustomerId = p.CustomerId ?? 0,
                BillId = p.BillId,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status
            }).ToList();
        }

        // Get payments by customer
        public async Task<List<PaymentDto>> GetByCustomerAsync(
            int customerId)
        {
            var payments = await _context.Payments
                .AsNoTracking()
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                PaymentReference = p.PaymentReference,
                CustomerId = p.CustomerId ?? 0,
                BillId = p.BillId,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status
            }).ToList();
        }

        // Create payment
        public async Task<PaymentDto> CreateAsync(
            PaymentCreateDto dto)
        {
            if (dto.CustomerId == null)
            {
                throw new ArgumentException(
                    "Customer ID is required.");
            }

            var payment = new Payment
            {
                PaymentReference =
                    $"PAY-{DateTime.Now:yyyyMMddHHmmssfff}",

                CustomerId = dto.CustomerId.Value,

                BillId = dto.BillId,

                PaymentDate = DateTime.Now,

                Amount = dto.Amount,

                PaymentMethod = dto.PaymentMethod,

                Status = "Successful"
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            return new PaymentDto
            {
                Id = payment.Id,
                PaymentReference = payment.PaymentReference,
                CustomerId = payment.CustomerId ?? 0,
                BillId = payment.BillId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status
            };
        }
    }
}