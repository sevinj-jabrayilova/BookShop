using BookShop.Models;
using BookShop.ViewModels.Categories;
using BookShop.ViewModels.Categories;

namespace BookShop.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryUIVM>> GetAllAsync();
        Task<IEnumerable<CategoryVM>> GetAllAdminAsync();
        Task<CategoryVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task CreateAsync(CreateCategoryVM model);
        Task UpdateAsync(UpdateCategoryVM model);
    }
}
