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
        private readonly IFileService _fileService;

        public ProductService(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
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

        public async Task CreateAsync(ProductCreateVM model)
        {
            List<ProductImage> images = new();

            foreach (var item in model.Images)
            {
                string fileName = _fileService.GenerateUniqueName(item.FileName);
                string path = _fileService.GeneratePath("client/assets/images", fileName);
                await _fileService.UploadAsync(item, path);

                images.Add(new ProductImage { Image = fileName });
            }

            if (images.Any())
                images.First().IsMain = true;

            var productCategories = model.CategoryIds?.Select(cid => new ProductCategory
            {
                CategoryId = cid
            }).ToList();

            Product product = new()
            {
                Name = model.Name,
                Author = model.Author,
                Description = model.Description,
                Price = model.Price,
                ProductImages = images,
                ProductCategories = productCategories
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var result = await _context.Products.FindAsync(id);

            if (result is null) return;

            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductDetailVM?> DetailAsync(int? id)
        {
            if (id is null) return null;

            var dbProduct = await _context.Products
                .Include(m => m.ProductCategories).ThenInclude(m => m.Category)
                                          .Where(m => m.Id == id)
                                          .Select(m => new ProductDetailVM
                                          {
                                              Name = m.Name,
                                              Description = m.Description,
                                              Price = m.Price,
                                              Categories = m.ProductCategories.Select(m => m.Category.Name).ToList(),
                                              Images = m.ProductImages.Select(m => new ProductImageDetailVM
                                              {
                                                  IsMain = m.IsMain,
                                                  Image = m.Image,

                                              }).ToList()
                                          }).FirstOrDefaultAsync();

            return dbProduct;
        }

        public async Task EditAsync(int id, ProductEditVM model)
        {
            var dbProduct = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dbProduct is null) return;
            if (model.NewImage != null)
            {
                var mainImage = dbProduct.ProductImages.FirstOrDefault(m => m.IsMain);

                string fileName = _fileService.GenerateUniqueName(model.NewImage.FileName);
                string newPath = _fileService.GeneratePath("client/assets/images", fileName);
                await _fileService.UploadAsync(model.NewImage, newPath);

                if (mainImage != null)
                {
                    string oldPath = _fileService.GeneratePath("client/assets/images", mainImage.Image);
                    _fileService.Delete(oldPath);

                    mainImage.Image = fileName;
                }
                else
                {
                    dbProduct.ProductImages.Add(new ProductImage
                    {
                        Image = fileName,
                        IsMain = true
                    });
                }
            }
            if (model.CategoryIds != null)
            {
                dbProduct.ProductCategories.Clear();

                var newCategories = model.CategoryIds.Select(cid => new ProductCategory
                {
                    ProductId = dbProduct.Id,
                    CategoryId = cid
                }).ToList();

                foreach (var cat in newCategories)
                {
                    dbProduct.ProductCategories.Add(cat);
                }
            }

            dbProduct.Name = model.Name;
            dbProduct.Description = model.Description;
            dbProduct.Price = model.Price;
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ProductVM>> GetAllAdminAsync()
        {
            return await _context.Products.Include(m => m.ProductCategories)
                .ThenInclude(m => m.Category)
                                          .Include(m => m.ProductImages)
                                          .Select(m => new ProductVM
                                          {
                                              Id = m.Id,
                                              Name = m.Name,
                                              Categories = m.ProductCategories.Select(m => m.Category.Name).ToList(),
                                              Image = m.ProductImages.FirstOrDefault(m => m.IsMain).Image,
                                          }).ToListAsync();
        }

        public async Task<ProductEditVM?> GetByIdAdminAsync(int id)
        {
            var dbProduct = await _context.Products.Include(m => m.ProductImages)
                .Include(m => m.ProductCategories).ThenInclude(m => m.Category)
                                                   .FirstOrDefaultAsync(m => m.Id == id);

            if (dbProduct == null) return null;

            return new ProductEditVM
            {
                Name = dbProduct.Name,
                Author  = dbProduct.Author,
                Description = dbProduct.Description,
                Price = dbProduct.Price,
                CategoryIds = dbProduct.ProductCategories.Select(m => m.CategoryId).ToList(),
                ExistImage = dbProduct.ProductImages.FirstOrDefault(m => m.IsMain)?.Image
            };
        }

        public async Task<decimal> GetPriceByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product.Price;
        }

        //edit, create hisselerde main image-ni deyismek ucun idi
        public async Task IsMainImage(int? id)
        {
            //if (id is null) return;

            //var data = await _context.ProductImages.FindAsync(id);

            //if(data is null) return;

            //var images = await _context.ProductImages.ToListAsync();
            //foreach (var item in images)
            //{
            //    item.IsMain = false;
            //}

            //data.IsMain = true;
            //_context.Update(data);
            //await _context.SaveChangesAsync();

            var selectedImage = await _context.ProductImages
                                     .Include(i => i.Product)
                                     .FirstOrDefaultAsync(i => i.Id == id);

            if (selectedImage == null) return;

            var productImages = await _context.ProductImages
                                              .Where(i => i.ProductId == selectedImage.ProductId)
                                              .ToListAsync();

            foreach (var image in productImages)
            {
                image.IsMain = false;
            }

            selectedImage.IsMain = true;

            await _context.SaveChangesAsync();
        }


    }
}
