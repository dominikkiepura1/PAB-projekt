using System;
using System.Collections.Generic;
using CarReservations.Application.Dto;

namespace CarReservations.Application.Interfaces
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetAllCars();
        CarDto? GetCarById(Guid id);
        Guid CreateCar(CarDto dto);
        void UpdateCar(CarDto dto);
        void DeleteCar(Guid id);
    }
}
