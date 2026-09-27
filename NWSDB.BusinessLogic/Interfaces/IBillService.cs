using System.Collections.Generic;
using System.Threading.Tasks;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface IBillService
    {
        Task<BillDto> CreateAsync(BillCreateDto dto);
        Task<List<BillDto>> GetAllAsync();
        Task<BillDto?> GetByIdAsync(int id);
        Task<List<BillDto>> GetByCustomerAsync(int customerId);
    }
}
