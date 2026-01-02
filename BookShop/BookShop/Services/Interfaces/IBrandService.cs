using BookShop.ViewModels.Brands;

namespace BookShop.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandUIVM>> GetAllAsync();
    }
}
