using System.Collections.Generic;
using System.Threading.Tasks;
using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetAllAsync();

        Task<List<PaymentDto>> GetByCustomerAsync(
            int customerId);

        Task<PaymentDto> CreateAsync(
            PaymentCreateDto dto);
    }
}
