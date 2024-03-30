using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class RegisterCustomerViewModel
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campus Name is required.")]
        public string CampusName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactNum { get; set; }

        [Required(ErrorMessage = "Type of Customer is required.")]
        public string TypeOfCustomer { get; set; }

        public string TypeOfRider { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms and conditions.")]
        public bool TAndCAgreement { get; set; }

        public bool EmailSubscription { get; set; }
    }
}
