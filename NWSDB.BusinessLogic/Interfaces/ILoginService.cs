using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface ILoginService
    {
        Task<(bool Success, string Message, string? Role, int UserId)> LoginAsync(
     LoginDto dto);
    }
}
