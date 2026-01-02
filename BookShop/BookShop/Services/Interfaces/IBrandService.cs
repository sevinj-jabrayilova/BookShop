using BookShop.ViewModels.Brands;
using BookShop.ViewModels.Sliders;
using System.Threading.Tasks;

namespace BookShop.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandUIVM>> GetAllAsync();
        Task<IEnumerable<BrandVM>> GetAllAdminAsync();
        Task CreateAsync(BrandCreateVM model);
        //Task DeleteAsync(int id);
        Task EditAsync(int id, BrandEditVM model);
        Task<BrandVM> GetByIdAsync(int id);
    }
}
