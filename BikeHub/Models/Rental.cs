using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeHub.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        // Foreign key attribute.
        [ForeignKey("Customer")]
        public int StudentId { get; set; }

        public string? BikeRented { get; set; }
        public DateTime DateRented { get; set; }
        public string? LockRented { get; set; }
        public string? BasketRented { get; set; }
        public string? KeyRented { get; set; }
        public string? Lights { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateReturned { get; set; }
        public int Duration { get; set; }
        public decimal Amount { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int KMSRidden { get; set; }
        public string? MethodOfTravel { get; set; }
        public int DaysLate { get; set; }
        public bool Banned { get; set; }
        public string? Notes { get; set; }
        public Customer? Customer { get; set; }

        // New attribute to track availability status
        // To be used to check Rented Bike
        public string AvailabilityStatus { get; set; } // "Rented",  "Retired", "Available"
        public string PaymentMethod { get; set; }


    }
}
