using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
