using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class Customer
    {
        [Key]
        [Required(ErrorMessage = "Id is required")]


        public int StudentId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string CampusName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNum { get; set; }
        public string TypeOfCustomer { get; set; }
        public string TypeOfRider { get; set; }
        public bool TAndCAgreement { get; set; }
        public bool EmailSubscription { get; set; }
    }
}