using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ProfileModel : PageModel
{
    public string FullName { get; set; } = "Rawihansi Bhagya";
    public string AccountNumber { get; set; } = "NWS0002";
    public string NIC { get; set; } = "200566556655";
    public string Email { get; set; } = "rawihansi@gmail.com";
    public string PhoneNumber { get; set; } = "0771234567";
    public string Address { get; set; } = "Kurunegala";
    public string AccountStatus { get; set; } = "Active";
    public DateTime RegistrationDate { get; set; } = new DateTime(2026, 09, 26);

    [BindProperty]
    public string? Message { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost(string? action)
    {
        if (action == "edit")
        {
            // Demo behavior only
            Message = "Profile editing will be available soon.";
        }

        return Page();
    }
}
