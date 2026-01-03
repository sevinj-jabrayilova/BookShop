namespace BookShop.Models
{
    public class Subscriber : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
