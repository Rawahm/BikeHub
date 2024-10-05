using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class RentalViewModel
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Customer Customer { get; set; }  

       public string Name { get; set; }

       [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Bike rented information is required.")]
        public string BikeRented { get; set; }

       [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DateRented { get; set; }

        [Required(ErrorMessage = "Lock rented information is required.")]
        public string LockRented { get; set; }

        [Required(ErrorMessage = "Basket rented information is required.")]
        public string BasketRented { get; set; }

        [Required(ErrorMessage = "Key rented information is required.")]
        public string KeyRented { get; set; }

        [Required(ErrorMessage = "Lights information is required.")]
        public string Lights { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DateReturned { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be greater than or equal to 0.")]

        public int Duration { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.")]
        public decimal Amount { get; set; }

        public string AvailabilityStatus { get; set; } // "Rented",  "Retired", "Available"
        public string PaymentMethod { get; set; }  //Cash,CIBC choices from the LISt

        [Required(ErrorMessage = "Paid status is required.")]
        public bool Paid { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "KMs ridden is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "KMs ridden must be greater than or equal to 0.")]
        public int KMSRidden { get; set; }

        public string MethodOfTravel { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Days must be greater than or equal to 0.")]

        public int DaysLate { get; set; }

        public bool Banned { get; set; }

        [Required(ErrorMessage = "Notes are required.")]
        public string Notes { get; set; }
    }
}
