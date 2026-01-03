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


        public async Task<IEnumerable<ProductGroupByCategoryVM>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .Select(p => new ProductUIVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Author = p.Author,
                    Image = p.ProductImages
                        .Where(i => i.IsMain)
                        .Select(i => i.Image)
                        .FirstOrDefault(),
                    CategoryIds = p.ProductCategories
                        .Select(c => c.CategoryId)
                        .ToList()
                })
                .ToListAsync();

            // Grupla ve tekilleştir
            var grouped = products
                .SelectMany(p => p.CategoryIds.Select(cid => new { Product = p, CategoryId = cid }))
                .GroupBy(x => x.CategoryId)
                .Select(g => new ProductGroupByCategoryVM
                {
                    CategoryId = g.Key,
                    Products = g
                        .Select(x => x.Product)
                        .GroupBy(p => p.Id) // Tekilleştir
                        .Select(gr => gr.First())
                        .ToList()
                })
                .ToList();

            return grouped;
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

        //public async Task<IEnumerable<ProductUIVM>> GetProductsWithOfferAsync(IEnumerable<int> basketProductIds)
        //{
        //    if (basketProductIds == null || !basketProductIds.Any())
        //        return Enumerable.Empty<ProductUIVM>();

        //    var popularCategoryIds = await _context.Products
        //        .Include(m => m.ProductCategories)
        //        .Where(p => basketProductIds.Contains(p.Id))
        //        .GroupBy(p => p.ProductCategories.Select(m => m.CategoryId))
        //        .OrderByDescending(g => g.Count())
        //        .Select(g => g.Key)
        //        .ToListAsync();

        //    if (popularCategoryIds.Count == 0)
        //        return Enumerable.Empty<ProductUIVM>();

        //    return await _context.Products
        //        .Include(m => m.ProductCategories)
        //        .Where(p => p.ProductCategories.Select(m => m.CategoryId) == popularCategoryIds)
        //        .Include(p => p.ProductImages)
        //        .Take(4)
        //        .Select(p => new ProductUIVM
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Author = p.Author,
        //            Price = p.Price,
        //            CategoryIds = p.ProductCategories.Select(m => m.CategoryId).ToList(),
        //            Image = p.ProductImages.FirstOrDefault(x => x.IsMain).Image
        //        })
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<ProductUIVM>> GetProductsWithOfferAsync(
    IEnumerable<int> basketProductIds)
        {
            if (basketProductIds == null || !basketProductIds.Any())
                return Enumerable.Empty<ProductUIVM>();

            var popularCategoryId = await _context.ProductCategories
                .Where(pc => basketProductIds.Contains(pc.ProductId))
                .GroupBy(pc => pc.CategoryId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (popularCategoryId == 0)
                return Enumerable.Empty<ProductUIVM>();

            return await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductImages)
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == popularCategoryId))
                .Take(4)
                .Select(p => new ProductUIVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Price = p.Price,
                    CategoryIds = p.ProductCategories
                        .Select(pc => pc.CategoryId)
                        .ToList(),
                    Image = p.ProductImages
                        .Where(img => img.IsMain)
                        .Select(img => img.Image)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


    }
}
