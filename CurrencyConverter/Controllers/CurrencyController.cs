using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverter.Services;
using CurrencyConverter.Models;
namespace CurrencyConverter.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var rates = await _currencyService.GetRatesAsync("USD");

                if (rates == null || rates.ConversionRates == null || !rates.ConversionRates.Any())
                {
                    ViewData["ErrorMessage"] = "Unable to fetch currency rates. Please try again later.";
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                return View(rates);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Error fetching currency rates: {ex.Message}";
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency) || amount <= 0)
                {
                    ViewData["ErrorMessage"] = "Invalid input values. Please check your data.";
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }
                var rates = await _currencyService.GetRatesAsync(fromCurrency);

                if (rates == null || rates.ConversionRates == null || !rates.ConversionRates.ContainsKey(toCurrency))
                {
                    ViewData["ErrorMessage"] = "Conversion rate not available for the selected currencies.";
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }
                var conversionRate = rates.ConversionRates[toCurrency];
                var result = amount * conversionRate;
                ViewBag.Result = result;
                ViewBag.Amount = amount;
                ViewBag.FromCurrency = fromCurrency;
                ViewBag.ToCurrency = toCurrency;
                return View("Index", rates);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Error during conversion: {ex.Message}";
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }
    }
}
