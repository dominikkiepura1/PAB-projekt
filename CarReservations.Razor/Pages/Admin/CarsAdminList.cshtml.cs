using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using CarReservations.Razor.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace CarReservations.Razor.Pages.Admin
{
    public class CarsAdminListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CarsAdminListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<CarVm> CarList { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            // Do ka¿dego wywo³ania dopisujemy nag³ówek Authorization
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/cars");
            if (!response.IsSuccessStatusCode)
            {
                // Opcjonalnie: przekieruj do strony b³êdu albo wyœwietl komunikat
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<List<CarVm>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (cars != null)
                CarList = cars;
        }
    }
}
