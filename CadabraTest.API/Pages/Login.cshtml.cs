using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using CadabraTest.API.Services;

public class LoginModel : PageModel
{
    private readonly AuthService  _authService; // your auth service

    public LoginModel(AuthService  authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        // any GET logic if needed
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Call your auth service to verify credentials
        var token = await _authService.LoginAsync(Email, Password);

        if (token != null)
        {
            // Save JWT in a cookie
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
            });

            return RedirectToPage("/Index"); // redirect after login
        }
        else
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }
    }
}
