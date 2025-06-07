using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarReservations.Razor.Pages.Admin
{
    public class CreateCarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CreateCarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CarVm Car { get; set; } = new CarVm();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("Api");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var payload = new
            {
                Brand = Car.Brand,
                Model = Car.Model,
                Year = Car.Year
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/cars", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "B³¹d tworzenia: " + errorBody);
                return Page();
            }

            // Tutaj koniecznie u¿ywamy pe³nej œcie¿ki od katalogu Pages:
            return RedirectToPage("/Admin/CarsAdminList");
        }
    }

    public class CarVm
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
    }
}
