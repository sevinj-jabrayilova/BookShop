using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Accounts
{
    public class LoginVM
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UsernameOrEmail { get; set; }
    }
}
