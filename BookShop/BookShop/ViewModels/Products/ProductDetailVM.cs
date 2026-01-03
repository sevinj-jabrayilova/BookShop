using BookShop.Models;

namespace BookShop.ViewModels.Products
{
    public class ProductDetailVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public ICollection<ProductImageDetailVM> Images { get; set; }
    }
}
