using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CarReservations.Razor.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LoginModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        [BindProperty]
        public LoginRequest LoginData { get; set; } = new LoginRequest();

        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("Api");
            var content = new StringContent(
                JsonSerializer.Serialize(LoginData),
                Encoding.UTF8, "application/json"
            );

            var response = await client.PostAsync("/api/auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "B³êdne dane logowania.";
                return Page();
            }

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (dto == null || string.IsNullOrEmpty(dto.Token))
            {
                ErrorMessage = "B³¹d w odpowiedzi z serwera.";
                return Page();
            }

            // Zapamiêtanie tokena i roli w sesji
            HttpContext.Session.SetString("JWToken", dto.Token);
            HttpContext.Session.SetString("UserRole", dto.Role);

            // Utworzenie Claimów i zalogowanie Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, LoginData.Username),
                new Claim(ClaimTypes.Role, dto.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(identity));

            // Przekierowanie zgodnie z rol¹
            if (dto.Role == "Admin")
            {
                return RedirectToPage("/Admin/CarsAdminList");
            }
            else
            {
                return RedirectToPage("/User/CarsUserList");
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
