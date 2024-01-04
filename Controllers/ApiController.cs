using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class ApiController : Controller
	{
		private readonly HttpClient httpClient;
		private readonly IUserService _userService;


		public ApiController(HttpClient httpClient, IUserService userService)
		{
			this.httpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
			_userService = userService;
		}
		public IActionResult Index()

		{
			return View();
		}
		public async Task<CurrencyModel> GetMetalPrice(string symbol, string curr, string year, string month, string day)
		{

			string apiKey = "bce54fad4f67965a35c9754582779520";
			string date = year + month + day;


			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			string url = $"https://api.metalpriceapi.com/v1/{year}-{month}-{day}?api_key={apiKey}&base=USD";


			try

			{
				var response = httpClient.GetAsync(url).Result;

				//response.EnsureSuccessStatusCode();
				string result = await response.Content.ReadAsStringAsync();
				var Metal = JsonConvert.DeserializeObject<CurrencyModel>(result);
				return Metal;
			}
			catch (Exception err)
			{
				Console.WriteLine($"blad {err.Message}");
				return null;
			}
		}
		public async Task<CurrencyModel> GetMetalPrice(string symbol, string curr)
		{

			string apiKey = "bce54fad4f67965a35c9754582779520";
			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			string url = $"https://api.metalpriceapi.com/v1/lasted?api_key={apiKey}&base=USD";
			try
			{
				var response = httpClient.GetAsync(url).Result;

				//response.EnsureSuccessStatusCode();
				string result = await response.Content.ReadAsStringAsync();
				var Metal = JsonConvert.DeserializeObject<CurrencyModel>(result);
				return Metal;

			}
			catch (Exception err)
			{
				Console.WriteLine($"blad {err.Message}");
				return null;
			}
		}
		public async Task<ConvertModel> Convert(string curr, string symbol, string year, string month, string day)
		{
			double ammount = await _userService.GetAmmountOfAsset(symbol, "Metal");
			string apiKey = "be2065fd505fe8b5783256a418ed0be6";
			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			var url = $"https://api.metalpriceapi.com/v1/convert?api_key={apiKey}&from={symbol}&to={curr}&amount={ammount}&date={year}-{month}-{day}";
			//string url = $"https://api.metalpriceapi.com/v1/2023-12-20";
			try
			{
				var response = httpClient.GetAsync(url).Result;


				string result = await response.Content.ReadAsStringAsync();
				var Metal = JsonConvert.DeserializeObject<ConvertModel>(result);
				return Metal;

			}
			catch (Exception err)
			{
				Console.WriteLine($"blad {err.Message}");
				return null;

			}
		}
		public async Task<Dictionary<string, string>> GetGetMetalDict()
		{
			Dictionary<string, string> MetalsDict = new Dictionary<string, string>
			{
			{ "XAU", "Gold" },
			{ "XAG", "Silver" },
			{ "XPT", "Platinum" },
			{ "XPD", "Palladium" },
			};
			return MetalsDict;
		}
		public async Task<Dictionary<string, string>> GetCurrencyDict()
		{
			Dictionary<string, string> CurrenciesDict = new Dictionary<string, string>
			{
			{"USD", "United States Dollar"},
			{"EUR", "Euro"},
			{"GBP", "British Pound Sterling"},
			{"JPY", "Japanese Yen"},
			{"AUD", "Australian Dollar"},
			{"CAD", "Canadian Dollar"},
			{"CHF", "Swiss Franc"},
			{"CNY", "Chinese Yuan"},
			{"SEK", "Swedish Krona"},
			{"NZD", "New Zealand Dollar"},
			{"NOK", "Norwegian Krone"},
			{"DKK", "Danish Krone"},
			{"SGD", "Singapore Dollar"},
			{"HKD", "Hong Kong Dollar"},
			{"KRW", "South Korean Won"},
			{"TRY", "Turkish Lira"},
			{"ZAR", "South African Rand"},
			{"SAR", "Saudi Riyal"},
			{"AED", "United Arab Emirates Dirham"},
			{"QAR", "Qatari Riyal"},
			{"MYR", "Malaysian Ringgit"},
			{"THB", "Thai Baht"},
			{"IDR", "Indonesian Rupiah"},
			{"INR", "Indian Rupee"},
			{"PHP", "Philippine Peso"},
			{"PKR", "Pakistani Rupee"},
			{"KWD", "Kuwaiti Dinar"},
			{"BHD", "Bahraini Dinar"},
			{"OMR", "Omani Rial"},
			{"JOD", "Jordanian Dinar"},
			{"PLN","Polish Zloty"}
			};
			return CurrenciesDict;
		}
		public async Task<Dictionary<string, string>> GetCryptoCurrencyDict()
		{
			Dictionary<string, string> CryptoDict = new Dictionary<string, string>
			{
			{ "BTC", "Bitcoin" },
			{ "ETH", "Ethereum" },
			{ "XRP", "Ripple" },
			{ "LTC", "Litecoin" },
			{ "BCH", "Bitcoin Cash" },
			{ "ADA", "Cardano" },
			{ "DOT", "Polkadot" },
			{ "XLM", "Stellar" },
			{ "DOGE", "Dogecoin" },
			{ "USDT", "Tether" },
			{ "XMR", "Monero" },
			{ "EOS", "EOS.IO" },
			{ "TRX", "TRON" },
			{ "XTZ", "Tezos" },
			{ "DASH", "Dash" },
			{ "ATOM", "Cosmos" },
			{ "LINK", "Chainlink" },
			{ "UNI", "Uniswap" },
			{ "AAVE", "Aave" },
			{ "SNX", "Synthetix" },
			};
			return CryptoDict;
		}
	}
}
