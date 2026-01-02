using BookShop.ViewModels.Sliders;

namespace BookShop.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderUIVM>> GetAllAsync();
        Task<IEnumerable<SliderVM>> GetAllAdminAsync();
        Task CreateAsync(SliderCreateVM model);
        Task DeleteAsync(int id);
        Task EditAsync(int id, SliderEditVM model);
        Task<SliderVM> GetByIdAsync(int id);
    }
}
