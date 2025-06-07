using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using CarReservations.Razor.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace CarReservations.Razor.Pages.Admin
{
    public class DeleteCarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DeleteCarModel(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        public CarVm? Car { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/cars/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToPage("CarsAdminList");

            var json = await response.Content.ReadAsStringAsync();
            Car = JsonSerializer.Deserialize<CarVm>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (Car == null)
                return RedirectToPage("CarsAdminList");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/cars/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // Mo¿esz dodaæ logikê b³êdu
                ModelState.AddModelError(string.Empty, "Nie uda³o siê usun¹æ samochodu.");
                return Page();
            }

            return RedirectToPage("CarsAdminList");
        }
    }
}
