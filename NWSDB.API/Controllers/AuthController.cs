using Microsoft.AspNetCore.Mvc;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

namespace NWSDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

      
        // LOGIN
        // POST: api/Auth/login
  
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    message = "Login data is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new
                {
                    message = "Email is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new
                {
                    message = "Password is required."
                });
            }

            try
            {
                var result =
                    await _authService.LoginAsync(dto);

                if (!result.Success)
                {
                    return Unauthorized(new
                    {
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    message = result.Message,
                    userId = result.UserId,
                    role = result.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An error occurred while processing the login.",
                        error = ex.Message
                    });
            }
        }

    
        // REGISTER
        // POST: api/Auth/register


        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    message = "Registration data is required."
                });
            }

            try
            {
                var result =
                    await _authService.RegisterAsync(dto);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message =
                            "An error occurred while processing the registration.",
                        error = ex.Message
                    });
            }
        }
    }
}