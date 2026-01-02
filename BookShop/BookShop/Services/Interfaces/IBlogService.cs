using BookShop.ViewModels.Blogs;

namespace BookShop.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogUIVM>> GetAllAsync();
    }
}
