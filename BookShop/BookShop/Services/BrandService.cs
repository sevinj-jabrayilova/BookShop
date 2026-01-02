using BookShop.Data;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Brands;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;

        public BrandService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BrandUIVM>> GetAllAsync()
        {
            return await _context.Brands.Select(m => new BrandUIVM { Image = m.Image }).ToListAsync();
        }
    }
}
