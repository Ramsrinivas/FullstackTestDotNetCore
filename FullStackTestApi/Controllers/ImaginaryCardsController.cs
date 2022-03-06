using System;
using System.Threading.Tasks;
using FullStackProgramTest.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FullStackTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaginaryCardsController : Controller
    {
        private readonly ILogger<ImaginaryCardsController> _logger;
        private readonly ICard _card;
        public ImaginaryCardsController(ILogger<ImaginaryCardsController> logger, ICard card)
        {
            _logger = logger;
            _card = card;
        }

        [HttpGet]
        [Route(nameof(GetSortedCards))]
        public async Task<IActionResult> GetSortedCards(string inputCardsDeck)
        {
            if (string.IsNullOrEmpty(inputCardsDeck))
            {
                return Json("NoInputValue");
            }
            try
            {
                (string sordtedCards, string wrongCards) = await _card.GetSortedCards(inputCardsDeck);

                return Json(sordtedCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(ex.Message);
            }
        }
    }
}
