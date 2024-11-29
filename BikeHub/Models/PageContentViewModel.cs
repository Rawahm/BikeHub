namespace BikeHub.Models
{
    public class PageContentViewModel
    {
        public int Id { get; set; }
        public string SelectedPageName { get; set; }
        public string SelectedSectionName { get; set; }
        public string Content { get; set; }

        // Dropdown List options
        public List<string> PageNames { get; set; } = new List<string> { "Home", "AboutUs" };
        public List<string> SectionNames { get; set; } = new List<string> { "Events", "About", "New" };
    }
}
