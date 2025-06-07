using System;

namespace CarReservations.Application.Dto
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsReserved { get; set; }
        public DateTime? ReservationFrom { get; set; }
        public DateTime? ReservationTo { get; set; }
        public Guid? ReservationId { get; set; }
    }
}
