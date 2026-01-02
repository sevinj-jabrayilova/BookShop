using BookShop.ViewModels.Blogs;
using BookShop.ViewModels.Brands;
using BookShop.ViewModels.Categories;
using BookShop.ViewModels.Products;
using BookShop.ViewModels.Sliders;

namespace BookShop.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderUIVM> Sliders { get; set; }
        public IEnumerable<BrandUIVM> Brands { get; set; }
        public IEnumerable<CategoryUIVM> Categories { get; set; }
        public IEnumerable<ProductUIVM> Products { get; set; }
        public IEnumerable<ProductUIVM> ProductsWithOffer { get; set; }
        public IEnumerable<BlogUIVM> Blogs { get; set; }

    }
}
