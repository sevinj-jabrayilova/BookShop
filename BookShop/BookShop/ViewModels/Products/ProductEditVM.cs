using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Products
{
    public class ProductEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }
        public string? ExistImage { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
