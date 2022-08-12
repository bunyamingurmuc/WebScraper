namespace WebScraperAPI.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryLink { get; set; }
        public string ParentCategory { get; set; }
        public int CategoryLevel { get; set; }
        public List<Filter> Filters { get; set; }


    }
}
