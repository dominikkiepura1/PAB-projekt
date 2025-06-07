using System;
using System.Collections.Generic;

namespace CarReservations.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
