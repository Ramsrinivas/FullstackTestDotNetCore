using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FullStackProgramTest.Contracts;
using FullStackProgramTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FullStackProgramTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICard _card;
        public HomeController(ILogger<HomeController> logger, ICard card)
        {
            _logger = logger;
            _card = card;
        }

        public async Task<IActionResult> Index(string cardsDeck)
        {
            if (string.IsNullOrEmpty(cardsDeck))
            {
                return View("/Error");
            }
            try
            {
                (string sordtedCards, string wrongCards) = await _card.GetSortedCards(cardsDeck);

                return View(sordtedCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //throw new NotImplementedException();
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
