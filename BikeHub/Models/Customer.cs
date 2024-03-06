using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class Customer
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CampusName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNum { get; set; }
        public string TypeOfCustomer { get; set; }
        public string TypeOfRider { get; set; }
        public string TAndCAgreement { get; set; }
        public string EmailSubscription { get; set; }   


    }
}


