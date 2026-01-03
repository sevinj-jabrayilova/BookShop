using BookShop.Models;

namespace BookShop.ViewModels.Products
{
    public class ProductImageDetailVM
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
