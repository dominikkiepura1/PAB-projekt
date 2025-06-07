using System;

namespace CarReservations.Application.Dto
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
