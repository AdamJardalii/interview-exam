using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CadabraTest.API.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; } = "Hello from code-behind";

        public void OnGet()
        {
            // This runs on GET requests
        }
    }
}
