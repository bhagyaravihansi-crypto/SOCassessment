using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;

public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Full Name is required.")]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Account Number is required.")]
    public string AccountNumber { get; set; } = string.Empty;

    [BindProperty]
    public string? NIC { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string? PhoneNumber { get; set; }

    [BindProperty]
    public string? Address { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please select a role.")]
    public string Role { get; set; } = "Customer";

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Please confirm your password.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? Message { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var dto = new RegisterUserDto
        {
            FullName = FullName,
            AccountNumber = AccountNumber,
            NIC = NIC,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Address = Address,
            Password = Password,
            Role = Role
        };

        var result = await _authService.RegisterAsync(dto);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return Page();
        }

        Message = result.Message;

        return Page();
    }
}