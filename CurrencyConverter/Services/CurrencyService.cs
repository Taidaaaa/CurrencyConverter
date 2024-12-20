namespace CurrencyConverter.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using CurrencyConverter.Models;
    public class CurrencyService
    {
        private readonly string apiUrlTemplate = "https://v6.exchangerate-api.com/v6/4a11fbe24e7a80248f1fbde8/latest/{0}";
        public async Task<CurrencyRates> GetRatesAsync(string baseCurrency)
        {
            var apiUrl = string.Format(apiUrlTemplate, baseCurrency);

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(apiUrl);
                    var rates = JsonConvert.DeserializeObject<CurrencyRates>(response);
                    if (rates == null || rates.ConversionRates == null)
                    {
                        return GetFallbackRates(baseCurrency);
                    }
                    return rates;
                }
                catch (Exception)
                {
                    return GetFallbackRates(baseCurrency);
                }
            }
        }
        private CurrencyRates GetFallbackRates(string baseCurrency)
        {
            return new CurrencyRates
            {
                Result = "failure",
                BaseCurrency = baseCurrency,
                ConversionRates = new Dictionary<string, decimal>
                {
                    { "EUR", 0.85m },
                    { "GBP", 0.75m },
                    { "JPY", 110.50m }
                }
            };
        }
    }
}
