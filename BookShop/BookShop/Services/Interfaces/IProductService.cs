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
        Task<IEnumerable<ProductVM>> GetAllAdminAsync();
        Task CreateAsync(ProductCreateVM model);
        Task<ProductDetailVM?> DetailAsync(int? id);
        Task DeleteAsync(int id);
        Task EditAsync(int id, ProductEditVM model);
        Task<ProductEditVM?> GetByIdAdminAsync(int id);
        Task IsMainImage(int? id);
    }
}
