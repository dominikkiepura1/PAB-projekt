using System;
using System.Collections.Generic;
using CarReservations.Application.Dto;
using CarReservations.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarReservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService) => _reservationService = reservationService;

        [HttpGet]
        public ActionResult<IEnumerable<ReservationDto>> GetAll() =>
            Ok(_reservationService.GetAllReservations());

        [HttpGet("{id}")]
        public ActionResult<ReservationDto> GetById(Guid id)
        {
            var res = _reservationService.GetReservationById(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public ActionResult<Guid> Create([FromBody] CreateReservationDto dto)
        {
            var newId = _reservationService.CreateReservation(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _reservationService.DeleteReservation(id);
            return NoContent();
        }
    }
}
