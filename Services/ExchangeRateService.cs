using ExchangeRatesAPI.Cache;
using ExchangeRatesAPI.Data;
using ExchangeRatesAPI.Helpers;
using ExchangeRatesAPI.Models;
using System.Text.Json;

namespace ExchangeRatesAPI.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MemoryCacheService _cacheService;
        private readonly LocalStorageService _localStorageService;
        private const string CacheKey = "ExchangeRates";
        private const string TcmbApiUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";

        public ExchangeRateService(IHttpClientFactory httpClientFactory, MemoryCacheService cacheService, LocalStorageService localStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _cacheService = cacheService;
            _localStorageService = localStorageService;
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(string currenyCode)
        {
            // TL Bazlı Döviz kurları 10 dakikalık süre ile TCMB üzerinden çekiliyor. 
            var cachedRates = _cacheService.GetFromCache<List<ExchangeRate>>(CacheKey);
            if (cachedRates == null)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetStringAsync(TcmbApiUrl);

                    var exchangeRates = XmlHelper.ParseExchangeRatesFromXml(response);
                    _cacheService.SetCache(CacheKey, exchangeRates, TimeSpan.FromMinutes(10));
                    await _localStorageService.SaveRatesToLocalFileAsync(exchangeRates);

                    cachedRates = exchangeRates;
                }
                catch
                {
                    // Hata durumunda lokalden veri çekiliyor.
                    cachedRates = await _localStorageService.LoadRatesFromLocalFileAsync();
                }
            }

            if (currenyCode == "TRY")
                return cachedRates;

            var convertCachedRates = new List<ExchangeRate>();
            convertCachedRates = _cacheService.GetFromCache<List<ExchangeRate>>(CacheKey + "_" + currenyCode);
            if (convertCachedRates != null)
                return convertCachedRates;

            convertCachedRates = new List<ExchangeRate>();

            var toBeConverted = cachedRates.Find(x => x.Code == currenyCode);
            if (toBeConverted != null)
            {
                foreach (var rt in cachedRates)
                {
                    var exchangeRate = new ExchangeRate
                    {
                        Code = rt.Code,
                        NameTR = rt.NameTR,
                        NameEN = rt.NameEN,
                        Rate =  rt.Code == toBeConverted.Code ? 1 : Math.Round((rt.Rate / toBeConverted.Rate), 4),
                        LastUpdateDate = DateTime.UtcNow
                    };
                    convertCachedRates.Add(exchangeRate);
                }
            }
            _cacheService.SetCache(CacheKey + "_" + currenyCode, convertCachedRates, TimeSpan.FromMinutes(1));
            return convertCachedRates;
        }
    }
}