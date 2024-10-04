using ExchangeRatesAPI.Models;
using System.Xml.Linq;

namespace ExchangeRatesAPI.Helpers
{
    public class XmlHelper
    {
        public static List<ExchangeRate> ParseExchangeRatesFromXml(string xmlContent)
        {
            List<string> allowExchangeRates = new List<string> { "USD", "EUR", "JPY", "GBP", "CNY", "AUD", "CAD", "CHF", "RUB" };
            var exchangeRates = new List<ExchangeRate>();

            exchangeRates.Add(
                new Models.ExchangeRate
                {
                    Code = "TRY",
                    NameTR = "TÜRK LİRASI",
                    NameEN = "TURKISH LIRA",
                    Rate = 1,
                    LastUpdateDate = DateTime.UtcNow
                });

            var document = XDocument.Parse(xmlContent);
            var currencies = document.Descendants("Currency");
            foreach (var currency in currencies)
            {
                string CurrencyCode = currency.Attribute("CurrencyCode")?.Value;
                if (allowExchangeRates.Contains(CurrencyCode))
                {
                    var exchangeRate = new ExchangeRate
                    {
                        Code = CurrencyCode,
                        NameTR = currency.Element("Isim")?.Value,
                        NameEN = currency.Element("CurrencyName")?.Value,
                        Rate = Math.Round((

                        (
                        (
                        ((double.TryParse(currency.Element("ForexBuying")?.Value.Replace(".", ","), out var forexBuying) ? forexBuying : 0) +
                        (double.TryParse(currency.Element("ForexSelling")?.Value.Replace(".", ","), out var forexSelling) ? forexSelling : 0))
                        ) / (double.TryParse(currency.Element("Unit")?.Value.Replace(".", ","), out var unit) ? unit : 1)
                        )

                        / 2), 4),
                        LastUpdateDate = DateTime.UtcNow
                    };
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
