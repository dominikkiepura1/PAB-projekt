using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CarReservations.Application.Dto;
using CarReservations.Application.Interfaces;
using CarReservations.Domain.Entities;
using CarReservations.Infrastructure.Data;
using System.Runtime.ConstrainedExecution;

namespace CarReservations.Infrastructure.Services
{
    public class CarService : ICarService
    {
        private readonly AppDbContext _context;
        public CarService(AppDbContext context) => _context = context;

        public IEnumerable<CarDto> GetAllCars()
        {
            return _context.Cars
                .Include(c => c.Reservations)
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    IsReserved = c.Reservations.Any(),
                    ReservationFrom = c.Reservations.OrderBy(r => r.From).Select(r => r.From).FirstOrDefault(),
                    ReservationTo = c.Reservations.OrderBy(r => r.From).Select(r => r.To).FirstOrDefault(),
                    ReservationId = c.Reservations.OrderBy(r => r.From).Select(r => r.Id).FirstOrDefault()
                })
                .ToList();
        }

        public CarDto? GetCarById(Guid id)
        {
            var c = _context.Cars.Include(car => car.Reservations).FirstOrDefault(car => car.Id == id);
            if (c == null) return null;
            return new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                IsReserved = c.Reservations.Any(),
                ReservationFrom = c.Reservations.OrderBy(r => r.From).Select(r => r.From).FirstOrDefault(),
                ReservationTo = c.Reservations.OrderBy(r => r.From).Select(r => r.To).FirstOrDefault(),
                ReservationId = c.Reservations.OrderBy(r => r.From).Select(r => r.Id).FirstOrDefault()
            };
        }

        public Guid CreateCar(CarDto dto)
        {
            var car = new Car { Id = Guid.NewGuid(), Brand = dto.Brand, Model = dto.Model, Year = dto.Year };
            _context.Cars.Add(car);
            _context.SaveChanges();
            return car.Id;
        }

        public void UpdateCar(CarDto dto)
        {
            var car = _context.Cars.Find(dto.Id);
            if (car == null) return;
            car.Brand = dto.Brand;
            car.Model = dto.Model;
            car.Year = dto.Year;
            _context.SaveChanges();
        }

        public void DeleteCar(Guid id)
        {
            var car = _context.Cars.Find(id);
            if (car != null) { _context.Cars.Remove(car); _context.SaveChanges(); }
        }
    }
}
