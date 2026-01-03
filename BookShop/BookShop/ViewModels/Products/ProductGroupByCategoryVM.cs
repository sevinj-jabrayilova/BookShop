using System.Collections;

namespace BookShop.ViewModels.Products
{
    public class ProductGroupByCategoryVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<ProductUIVM> Products { get; set; }
    }
}
