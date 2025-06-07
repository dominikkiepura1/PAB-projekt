using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarReservations.Razor.Pages.User
{
    public class CarsUserListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CarsUserListModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;
        public List<CarVm> CarList { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));
            var response = await client.GetAsync("/api/cars");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            CarList = JsonSerializer.Deserialize<List<CarVm>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }

    public record CarVm(Guid Id, string Brand, string Model, int Year, bool IsReserved, DateTime? ReservationFrom, DateTime? ReservationTo, Guid? ReservationId);
}
