using Newtonsoft.Json;
using System.Collections.Generic;
namespace CurrencyConverter.Models
{
    public class CurrencyRates
    {
        [JsonProperty("result")]
        public string Result { get; set; } = string.Empty;
        [JsonProperty("base_code")]
        public string BaseCurrency { get; set; } = string.Empty;
        [JsonProperty("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; } = new Dictionary<string, decimal>();
    }
}
