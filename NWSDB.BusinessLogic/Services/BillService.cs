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
    public class BillService : IBillService
    {
        private readonly NwsdbContext _db;

        public BillService(NwsdbContext db)
        {
            _db = db;
        }

        public async Task<BillDto> CreateAsync(BillCreateDto dto)
        {
            var bill = new Bill
            {
                CustomerId = dto.CustomerId,
                BillingPeriod = dto.BillingPeriod,
                PreviousReading = dto.PreviousReading,
                CurrentReading = dto.CurrentReading,
                UnitsConsumed = dto.CurrentReading - dto.PreviousReading,
                UsageCharge = dto.UsageCharge,
                ServiceCharge = dto.ServiceCharge,
                OtherCharges = dto.OtherCharges,
            };
            bill.TotalAmount = bill.UsageCharge + bill.ServiceCharge + bill.OtherCharges;
            bill.BillNumber = $"BILL-{System.DateTime.UtcNow.Ticks % 100000}";

            await _db.Bills.AddAsync(bill);
            await _db.SaveChangesAsync();

            // Load customer name if available
            await _db.Entry(bill).Reference(b => b.Customer).LoadAsync();

            return Map(bill);
        }

        public async Task<List<BillDto>> GetAllAsync()
        {
            var list = await _db.Bills.Include(b => b.Customer).AsNoTracking().ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<BillDto?> GetByIdAsync(int id)
        {
            var b = await _db.Bills.Include(x => x.Customer).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return null;
            return Map(b);
        }

        public async Task<List<BillDto>> GetByCustomerAsync(int customerId)
        {
            var list = await _db.Bills.Include(b => b.Customer).AsNoTracking().Where(b => b.CustomerId == customerId).ToListAsync();
            return list.Select(Map).ToList();
        }

        private static BillDto Map(Bill b)
        {
            return new BillDto
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                CustomerId = b.CustomerId,
                CustomerName = b.Customer?.FullName ?? string.Empty,
                BillingPeriod = b.BillingPeriod,
                IssueDate = b.IssueDate,
                DueDate = b.DueDate,
                PreviousReading = b.PreviousReading,
                CurrentReading = b.CurrentReading,
                UnitsConsumed = b.UnitsConsumed,
                UsageCharge = b.UsageCharge,
                ServiceCharge = b.ServiceCharge,
                OtherCharges = b.OtherCharges,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            };
        }
    }
}
