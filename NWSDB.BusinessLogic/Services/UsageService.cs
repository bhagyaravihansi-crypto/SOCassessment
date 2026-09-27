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
    public class UsageService : IUsageService
    {
        private readonly NwsdbContext _db;

        public UsageService(NwsdbContext db)
        {
            _db = db;
        }

        public async Task<UsageDto> CreateAsync(UsageDto dto)
        {
            var rec = new UsageRecord
            {
                CustomerId = dto.CustomerId,
                BillingMonth = dto.BillingMonth,
                PreviousReading = dto.PreviousReading,
                CurrentReading = dto.CurrentReading,
                UnitsConsumed = dto.UnitsConsumed,
                ReadingDate = dto.ReadingDate,
                UsageStatus = dto.UsageStatus
            };

            await _db.UsageRecords.AddAsync(rec);
            await _db.SaveChangesAsync();

            await _db.Entry(rec).Reference(r => r.Customer).LoadAsync();

            return Map(rec);
        }

        public async Task<List<UsageDto>> GetUsageRecordsAsync()
        {
            var list = await _db.UsageRecords.Include(u => u.Customer).AsNoTracking().ToListAsync();
            return list.Select(Map).ToList();
        }

        public async Task<List<UsageDto>> GetByCustomerAsync(int customerId)
        {
            var list = await _db.UsageRecords.Include(u => u.Customer).AsNoTracking().Where(u => u.CustomerId == customerId).ToListAsync();
            return list.Select(Map).ToList();
        }

        private static UsageDto Map(UsageRecord r)
        {
            return new UsageDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer?.FullName ?? string.Empty,
                BillingMonth = r.BillingMonth,
                PreviousReading = r.PreviousReading,
                CurrentReading = r.CurrentReading,
                UnitsConsumed = r.UnitsConsumed,
                ReadingDate = r.ReadingDate,
                UsageStatus = r.UsageStatus
            };
        }
    }
}
