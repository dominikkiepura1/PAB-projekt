using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CarReservations.Razor.Pages.Auth
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Usuń dane z sesji
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Remove("UserRole");

            // Wyloguj cookie‐authentication
            await HttpContext.SignOutAsync();

            // Przekieruj do /Auth/Login
            return RedirectToPage("/Auth/Login");
        }
    }
}
