using Microsoft.AspNetCore.Identity;

namespace BookShop.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public Subscriber Subscriber { get; set; }
    }
}
