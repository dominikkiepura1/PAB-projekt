using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarReservations.Razor.Pages.User
{
    public class ReserveModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ReserveModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        [BindProperty] public DateTime From { get; set; } = DateTime.Today;
        [BindProperty] public DateTime To { get; set; } = DateTime.Today;
        public Guid CarId { get; set; }

        public void OnGet(Guid id) => CarId = id;

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var payload = new
            {
                CarId = id,
                CustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // lub z claims
                From,
                To
            };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/reservations", content);
            response.EnsureSuccessStatusCode();
            return RedirectToPage("/User/CarsUserList");
        }
    }
}
