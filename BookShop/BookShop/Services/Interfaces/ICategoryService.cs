using BookShop.ViewModels.Categories;

namespace BookShop.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryUIVM>> GetAllAsync();
    }
}
