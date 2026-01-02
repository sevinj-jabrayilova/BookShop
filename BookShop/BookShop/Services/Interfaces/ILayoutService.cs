namespace BookShop.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<Dictionary<string, string>> GetAllSettingsAsync();
    }
}
