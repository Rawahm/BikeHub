namespace BikeHub.Models
{
    public class ContactUs
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now; // Set Default to current date
    }
}
