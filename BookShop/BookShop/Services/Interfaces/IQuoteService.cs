using BookShop.ViewModels.Quotes;

namespace BookShop.Services.Interfaces
{
    public interface IQuoteService
    {
        Task<QuoteUIVM> GetQuoteAsync();
    }
}
