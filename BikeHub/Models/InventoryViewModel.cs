using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class InventoryViewModel
    {
        [Required(ErrorMessage = "Item ID is required.")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item Type is required.")]
        public string ItemType { get; set; }

        [Required(ErrorMessage = "Item Number is required.")]
        public string ItemNumber { get; set; }

        [Required(ErrorMessage = "Condition is required.")]

        public string Condition { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Availability Status is required.")]

        public string AvailabilityStatus { get; set; }


        public string Comments { get; set; }
        public string RetiredOptions { get; set; } //  property for retired options

    }
}
