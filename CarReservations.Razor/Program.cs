using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5219");
})
.ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        // UWAGA: tylko do developmentu!
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options => options.LoginPath = "/Auth/Login");

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// (� konfiguracja HttpClient, sesji i autoryzacji, jak w poprzednich krokach �)
builder.Services.AddRazorPages(options =>
{
    // Blokujemy widoki admina dla niezalogowanych
    options.Conventions.AuthorizeFolder("/Admin");
    // Login i Logout dost�pne anonimowo
    options.Conventions.AllowAnonymousToPage("/Auth/Login");
    options.Conventions.AllowAnonymousToPage("/Auth/Logout");
});

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
