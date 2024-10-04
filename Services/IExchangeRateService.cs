using ExchangeRatesAPI.Models;

namespace ExchangeRatesAPI.Services
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRate>> GetExchangeRatesAsync(string currenyCode);
    }
}
