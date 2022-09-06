using Cards.Api.Data;
using Cards.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext _cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            _cardsDbContext = cardsDbContext;
        }
        //GetAllCards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //GetCard
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
            return Ok(card);
            }

            return NotFound("Card not found");
        }

        //AddCard
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            if (card != null)
            {
                card.Id = Guid.NewGuid();
                await _cardsDbContext.Cards.AddAsync(card);
                await _cardsDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCard), new { Id = card.Id }, card);
            }

            return NotFound("Card not valid");
        }

        //UpdateCard
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id,[FromBody] Card card)
        {
            if (card != null)
            {
                var _card = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == card.Id);
                if (_card != null)
                {
                    _card.CardHolderName = card.CardHolderName;
                    _card.CardNumber = card.CardNumber;
                    _card.ExpiryMonth = card.ExpiryMonth;
                    _card.ExpiryYear = card.ExpiryYear;
                    _card.CVC = card.CVC;

                    await _cardsDbContext.SaveChangesAsync();

                    return Ok(_card);
                }
                return NotFound("Card not found");
            }
            return NotFound("Card not valid");
        }

        //DeleteCard
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var card = await _cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                _cardsDbContext.Cards.Remove(card);
                await _cardsDbContext.SaveChangesAsync();
                return Ok();
            }

            return NotFound("Card not found");
        }
    }
}
