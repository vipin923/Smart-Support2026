namespace Smart_Support2026.Models
{
    public class Menu
    {
        public class Links
        {
            public int Id { get; set; } 
            public string Role { get; set; } = string.Empty;
            public string Link { get; set; } = string.Empty;
            public List<SubLinks> SubLinks { get; set; } = new();

        }
        public class SubLinks
        {
            public int SubLinkId { get; set; }
            public string Sub_Link { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
        }
    }
}
