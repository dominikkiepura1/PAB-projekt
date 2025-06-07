using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CarReservations.Razor.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace CarReservations.Razor.Pages.Admin
{
    public class EditCarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EditCarModel(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        [BindProperty]
        public CarVm Car { get; set; } = new();

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
            var dto = JsonSerializer.Deserialize<CarVm>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (dto == null)
                return RedirectToPage("CarsAdminList");

            Car = dto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("Api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                Id = Car.Id,
                Brand = Car.Brand,
                Model = Car.Model,
                Year = Car.Year
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/cars/{Car.Id}", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zaktualizowaæ samochodu.");
                return Page();
            }

            return RedirectToPage("CarsAdminList");
        }
    }
}
