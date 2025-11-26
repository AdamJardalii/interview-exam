using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SignupModel : PageModel
{
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        // Call your AuthService to create the user
        // Redirect to Login after signup
        return RedirectToPage("/Login");
    }
}
