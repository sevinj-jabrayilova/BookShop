using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Brands;
using BookShop.ViewModels.Sliders;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public BrandService(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IEnumerable<BrandVM>> GetAllAdminAsync()
        {
            return await _context.Brands.Select(m => new BrandVM { Id= m.Id, Image = m.Image }).ToListAsync();
        }

        public async Task<IEnumerable<BrandUIVM>> GetAllAsync()
        {
            return await _context.Brands.Select(m => new BrandUIVM { Image = m.Image }).ToListAsync();
        }
        public async Task<BrandVM> GetByIdAsync(int id)
        {
            var dbBrand = await _context.Brands.FindAsync(id);
            return new BrandVM
            {
                Id = dbBrand.Id,
                Image = dbBrand.Image
            };
        }

        public async Task CreateAsync(BrandCreateVM model)
        {
            foreach (var image in model.NewImages)
            {
                string fileName = _fileService.GenerateUniqueName(image.FileName);

                string path = _fileService.GeneratePath("client/assets/images", fileName);

                await _fileService.UploadAsync(image, path);

                await _context.AddAsync(new Brand
                {
                    Image = fileName
                });
            }
            await _context.SaveChangesAsync();
        }

        //public async Task CreateAsync(SliderCreateVM model)
        //{
        //    foreach (var image in model.NewImages)
        //    {
        //        string fileName = _fileService.GenerateUniqueName(image.FileName);

        //        string path = _fileService.GeneratePath("client/assets/images", fileName);

        //        await _fileService.UploadAsync(image, path);

        //        await _context.AddAsync(new Slider
        //        {
        //            Image = fileName,
        //            Title = model.Title,
        //            Description = model.Description
        //        });
        //    }
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var dbSlider = await _context.Sliders.FindAsync(id);
        //    if (dbSlider is null) return;

        //    string existPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client", "assets", "images", dbSlider.Image);
        //    if (System.IO.File.Exists(existPath))
        //        _fileService.Delete(existPath);

        //    _context.Sliders.Remove(dbSlider);
        //    await _context.SaveChangesAsync();
        //}

        public async Task EditAsync(int id, BrandEditVM model)
        {
            var dbSlider = await _context.Brands.FindAsync(id);

            if (model.NewImage != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client", "assets", "images");

                string oldPath = _fileService.GeneratePath("client/assets/images", dbSlider.Image);
                _fileService.Delete(oldPath);

                string fileName = _fileService.GenerateUniqueName(model.NewImage.FileName);
                string newPath = Path.Combine(folderPath, fileName);
                await _fileService.UploadAsync(model.NewImage, newPath);

                dbSlider.Image = fileName;
            }


            await _context.SaveChangesAsync();
        }

    }
}
