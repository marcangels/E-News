namespace E_News
{
    public class GuardianArticle
    {
        public string id { get; set; }
        public string type { get; set; }
        public string sectionId { get; set; }
        public string sectionName { get; set; }
        public string webPublicationDate { get; set; }
        public string webTitle { get; set; }
        public string webUrl { get; set; }
        public string apiUrl { get; set; }
        public GuardianBody fields { get; set; }
        public bool isHosted { get; set; }
    }
}