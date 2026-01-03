using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Categories
{
    public class UpdateCategoryVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
