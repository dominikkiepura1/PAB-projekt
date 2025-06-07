using System;

namespace CarReservations.Application.Dto
{
    public class CreateReservationDto
    {
        public Guid CarId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
