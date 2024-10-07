using System.ComponentModel.DataAnnotations;

namespace BikeHub.Models
{
    public class ArchivedInventory
    {

        [Key]
        public int ItemId { get; set; }

        [Required]
        public string ItemType { get; set; }

        [Required]
        public string ItemNumber { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Required]
        public string AvailabilityStatus { get; set; }

        public string Comments { get; set; }
        public string? RetiredOptions { get; set; }
        public DateTime ArchivedDate { get; set; }         // Date when the item was archived


    }
}
