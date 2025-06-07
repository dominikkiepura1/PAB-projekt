using System;
using System.Collections.Generic;
using CarReservations.Application.Dto;

namespace CarReservations.Application.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<ReservationDto> GetAllReservations();
        ReservationDto? GetReservationById(Guid id);
        Guid CreateReservation(CreateReservationDto dto);
        void DeleteReservation(Guid id);
    }
}
