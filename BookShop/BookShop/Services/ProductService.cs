using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Products;
using BookShop.ViewModels.Sliders;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<ProductUIVM>> GetAllAsync()
        //{
        //    return await _context.Products
        //        .Include(p => p.ProductImages)
        //        .Include(p => p.ProductCategories)
        //        .Select(p => new ProductUIVM
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Price = p.Price,
        //            Author = p.Author,

        //            Image = p.ProductImages.FirstOrDefault(m => m.IsMain).Image,

        //            CategoryId = p.ProductCategories
        //                .Select(pc => pc.CategoryId)
        //                .ToList()
        //        })
        //        .ToListAsync();
        //}


        public async Task<IEnumerable<ProductUIVM>> GetAllAsync()
        {
            return await _context.Products.Include(m => m.ProductImages).Select(m => new ProductUIVM
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                Author = m.Author,
                CategoryId = m.CategoryId,
                Image = m.ProductImages.FirstOrDefault(m => m.IsMain).Image
            }).ToListAsync();
        }

        public async Task<BestSellingProductUIVM> GetBestSellingProductAsync()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.SellCount)
                .Select(p => new BestSellingProductUIVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.ProductImages.FirstOrDefault(x => x.IsMain).Image
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<ProductUIVM>> GetProductsWithOfferAsync(IEnumerable<int> basketProductIds)
        {
            if (basketProductIds == null || !basketProductIds.Any())
                return Enumerable.Empty<ProductUIVM>();

            var popularCategoryId = await _context.Products
                .Where(p => basketProductIds.Contains(p.Id))
                .GroupBy(p => p.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (popularCategoryId == 0)
                return Enumerable.Empty<ProductUIVM>();

            return await _context.Products
                .Where(p => p.CategoryId == popularCategoryId)
                .Include(p => p.ProductImages)
                .Take(4)
                .Select(p => new ProductUIVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Image = p.ProductImages.FirstOrDefault(x => x.IsMain).Image
                })
                .ToListAsync();
        }

    }
}
