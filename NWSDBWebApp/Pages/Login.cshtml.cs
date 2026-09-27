using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NWSDB.BusinessLogic.DTOs;
using NWSDB.BusinessLogic.Interfaces;

public class LoginModel : PageModel
{
    private readonly ILoginService _loginService;

    public LoginModel(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Please enter your account number or email")]
    public string? Identifier { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [BindProperty]
    public bool RememberMe { get; set; }

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

        var dto = new LoginDto
        {
            Email = Identifier!,
            Password = Password!
        };

        var result = await _loginService.LoginAsync(dto);

        if (!result.Success)
        {
            Message = result.Message;
            Password = null;
            return Page();
        }

        // Save logged-in user's ID in Session
        HttpContext.Session.SetInt32("UserId", result.UserId);

        // Save user's role in Session
        HttpContext.Session.SetString("UserRole", result.Role ?? "");

        if (result.Role == "Admin")
        {
            return RedirectToPage("/Admin/Dashboard");
        }

        return RedirectToPage("/Customer/Dashboard");
    }
}