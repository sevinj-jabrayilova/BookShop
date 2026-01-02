using Microsoft.AspNetCore.Identity;

namespace BookShop.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
