using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CarReservations.Razor.Pages.User
{
    public class CancelReservationModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CancelReservationModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            var response = await client.DeleteAsync($"/api/reservations/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê anulowaæ rezerwacji.");
                return Page();
            }
            return RedirectToPage("/User/CarsUserList");
        }
    }
}
