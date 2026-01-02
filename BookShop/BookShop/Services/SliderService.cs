using BookShop.Data;
using BookShop.Models;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Sliders;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public SliderService(AppDbContext context,
            IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task CreateAsync(SliderCreateVM model)
        {
            foreach (var image in model.NewImages)
            {
                string fileName = _fileService.GenerateUniqueName(image.FileName);

                string path = _fileService.GeneratePath("client/assets/images", fileName);

                await _fileService.UploadAsync(image, path);

                await _context.AddAsync(new Slider { Image = fileName,
                                                     Title = model.Title,
                                                     Description = model.Description});
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dbSlider = await _context.Sliders.FindAsync(id);
            if (dbSlider is null) return;

            string existPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client", "assets", "images", dbSlider.Image);
            if (System.IO.File.Exists(existPath))
                _fileService.Delete(existPath);

            _context.Sliders.Remove(dbSlider);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, SliderEditVM model)
        {
            var dbSlider = await _context.Sliders.FindAsync(id);

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

            dbSlider.Title = model.Title;
            dbSlider.Description = model.Description;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SliderVM>> GetAllAdminAsync()
        {
            return await _context.Sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image }).ToListAsync();
        }

        public async Task<IEnumerable<SliderUIVM>> GetAllAsync()
        {
            return await _context.Sliders.Select(m => new SliderUIVM { 
                Image = m.Image,
                Title = m.Title,
                Description = m.Description
            }).ToListAsync();
        }

        public async Task<SliderVM> GetByIdAsync(int id)
        {
            var dbSldier = await _context.Sliders.FindAsync(id);
            return new SliderVM { 
                Id = dbSldier.Id, 
                Image = dbSldier.Image, 
                Description = dbSldier.Description, 
                Title = dbSldier.Title 
            };
        }
    }
}
