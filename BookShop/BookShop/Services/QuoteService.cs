using BookShop.Data;
using BookShop.Services.Interfaces;
using BookShop.ViewModels.Quotes;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly AppDbContext _context;

        public QuoteService (AppDbContext context)
        {
            _context = context;
        }
        public async Task<QuoteUIVM> GetQuoteAsync()
        {
            var today = DateTime.Today;

            var quoteOfTheDay = await _context.Quotes
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Date.Date == today);

            if (quoteOfTheDay is null) return null;

            return new QuoteUIVM
            {
                Author = quoteOfTheDay.Author,
                Content = quoteOfTheDay.Content
            };

        }
    }
}
