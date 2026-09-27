using System.Collections.Generic;
using System.Threading.Tasks;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface IUsageService
    {
        Task<UsageDto> CreateAsync(NWSDB.BusinessLogic.DTOs.UsageDto dto);
        Task<List<UsageDto>> GetUsageRecordsAsync();
        Task<List<UsageDto>> GetByCustomerAsync(int customerId);
    }
}

