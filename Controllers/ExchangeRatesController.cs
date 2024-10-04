using ExchangeRatesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRatesController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("GetExchangeRates")]
        public async Task<IActionResult> GetExchangeRates([FromQuery] string currencyCode)
        {
            var rates = await _exchangeRateService.GetExchangeRatesAsync(currencyCode);
            if (rates == null) 
                return StatusCode(500, "Veri çekilemedi.");
            return Ok(rates);
        }
    }
}
