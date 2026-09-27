using System.Collections.Generic;
using System.Threading.Tasks;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(CustomerCreateDto dto);

        Task<List<CustomerDto>> GetAllAsync();

        Task<CustomerDto?> GetByIdAsync(int id);

        Task<CustomerDto?> GetByUserIdAsync(int userId);

        Task<bool> UpdateAsync(int id, CustomerUpdateDto dto);
    }
}
