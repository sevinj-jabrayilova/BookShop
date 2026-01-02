namespace BookShop.Models
{
    public class Quote : BaseEntity
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }
}
