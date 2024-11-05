namespace BikeHub.Models
{
    public class PageContent
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string SectionName { get; set; }
        public string Content { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        
        }
}
