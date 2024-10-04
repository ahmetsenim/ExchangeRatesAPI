using ExchangeRatesAPI.Models;
using System.Text.Json;

namespace ExchangeRatesAPI.Data
{
    public class LocalStorageService
    {
        private const string FilePath = "local_exchange_rates.json";

        public async Task SaveRatesToLocalFileAsync(List<ExchangeRate> exchangeRates)
        {
            var json = JsonSerializer.Serialize(exchangeRates);
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task<List<ExchangeRate>> LoadRatesFromLocalFileAsync()
        {
            if (!File.Exists(FilePath))
                return null;

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<ExchangeRate>>(json);
        }
    }
}
