using System;
using System.Collections.Generic;
using System.Linq;
using CarReservations.Application.Dto;
using CarReservations.Application.Interfaces;
using CarReservations.Domain.Entities;
using CarReservations.Infrastructure.Data;

namespace CarReservations.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        public ReservationService(AppDbContext context) => _context = context;

        public IEnumerable<ReservationDto> GetAllReservations() =>
            _context.Reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CarId = r.CarId,
                CustomerId = r.CustomerId,
                From = r.From,
                To = r.To
            }).ToList();

        public ReservationDto? GetReservationById(Guid id)
        {
            var r = _context.Reservations.Find(id);
            if (r == null) return null;
            return new ReservationDto
            {
                Id = r.Id,
                CarId = r.CarId,
                CustomerId = r.CustomerId,
                From = r.From,
                To = r.To
            };
        }

        public Guid CreateReservation(CreateReservationDto dto)
        {
            var r = new Reservation
            {
                Id = Guid.NewGuid(),
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                From = dto.From,
                To = dto.To
            };
            _context.Reservations.Add(r);
            _context.SaveChanges();
            return r.Id;
        }

        public void DeleteReservation(Guid id)
        {
            var r = _context.Reservations.Find(id);
            if (r != null) { _context.Reservations.Remove(r); _context.SaveChanges(); }
        }
    }
}
