using BookShop.Data;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryUIVM>> GetAllAsync()
        {
            return await _context.Categories.Select(c => new CategoryUIVM
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
        }
    }
}
