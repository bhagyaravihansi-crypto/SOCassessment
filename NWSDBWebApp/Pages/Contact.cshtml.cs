using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ContactModel : PageModel
{
    [BindProperty]
    [Required]
    public string? FullName { get; set; }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [BindProperty]
    [Required]
    public string? Subject { get; set; }

    [BindProperty]
    [Required]
    public string? Message { get; set; }

    public string? StatusMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Demo only: do not send email or save data
        StatusMessage = "Thank you for contacting NWSDB. Your message has been received.";

        // Clear form fields
        FullName = null;
        Email = null;
        Subject = null;
        Message = null;

        // Clear model state so the fields appear empty
        ModelState.Clear();

        return Page();
    }
}
