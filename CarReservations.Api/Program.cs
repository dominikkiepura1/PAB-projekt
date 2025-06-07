using CarReservations.Domain.Entities;
using CarReservations.Application.Interfaces;
using CarReservations.Infrastructure.Services;
using CarReservations.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;

var builder = WebApplication.CreateBuilder(args);

// JWT settings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var issuer = jwtSettingsSection["Issuer"];
var audience = jwtSettingsSection["Audience"];
var secretKey = jwtSettingsSection["SecretKey"];

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CarReservationsDb"));

// Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// Services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Customers.Any())
    {
        context.Customers.Add(new Customer
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@example.com"
        });
    }
    if (!context.Cars.Any())
    {
        context.Cars.AddRange(
            new Car { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Brand = "Toyota", Model = "Corolla", Year = 2020 },
            new Car { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Brand = "Tesla", Model = "Model 3", Year = 2022 }
        );
    }
    if (!context.Reservations.Any())
    {
        context.Reservations.Add(new Reservation
        {
            Id = Guid.NewGuid(),
            CarId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            From = new DateTime(2025, 4, 7),
            To = new DateTime(2025, 4, 10)
        });
    }
    context.SaveChanges();
}

app.Run();
