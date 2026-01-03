using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.Products
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        //public int CategoryId { get; set; }
        [Required]
        public IEnumerable<int> CategoryIds { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
