using System;
using System.Collections.Generic;
using CarReservations.Application.Dto;
using CarReservations.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarReservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarsController(ICarService carService) => _carService = carService;

        [HttpGet]
        public ActionResult<IEnumerable<CarDto>> GetAll() => Ok(_carService.GetAllCars());

        [HttpGet("{id}")]
        public ActionResult<CarDto> GetById(Guid id)
        {
            var car = _carService.GetCarById(id);
            return car is null ? NotFound() : Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Guid> Create([FromBody] CarDto dto)
        {
            var newId = _carService.CreateCar(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CarDto dto)
        {
            if (id != dto.Id) return BadRequest();
            _carService.UpdateCar(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _carService.DeleteCar(id);
            return NoContent();
        }
    }
}
