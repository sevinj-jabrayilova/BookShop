using BookShop.Models;
using BookShop.ViewModels.Products;
using BookShop.ViewModels.Sliders;

namespace BookShop.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductGroupByCategoryVM>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<BestSellingProductUIVM> GetBestSellingProductAsync();
        Task<IEnumerable<ProductUIVM>> GetProductsWithOfferAsync(IEnumerable<int> basketProductIds);
    }
}
