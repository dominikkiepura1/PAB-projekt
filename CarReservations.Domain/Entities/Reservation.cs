using System;

namespace CarReservations.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Car? Car { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
