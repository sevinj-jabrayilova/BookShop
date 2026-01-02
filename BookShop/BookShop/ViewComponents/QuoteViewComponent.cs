using BookShop.Services;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class QuoteViewComponent : ViewComponent
    {
        private readonly IQuoteService _quoteService;

        public QuoteViewComponent(IQuoteService quoteservice)
        {
            _quoteService = quoteservice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var quote = await _quoteService.GetQuoteAsync();
            return View(quote);
        }

    }
}
