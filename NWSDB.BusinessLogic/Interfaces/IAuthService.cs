using NWSDB.BusinessLogic.DTOs;

namespace NWSDB.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(
            RegisterUserDto dto);

        Task<(bool Success, string Message, int UserId, string Role)>
            LoginAsync(LoginDto dto);
    }
}