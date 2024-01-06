using PortfolioApp.Components.Services.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using PortfolioApp.Models;
using Microsoft.AspNetCore.Identity;
using PortfolioApp.Services.Interfaces;
using Humanizer;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PortfolioApp.Services
{
	public class ApiService : IApiService
	{
		//private const string COIN_API_KEY = "a8fb9c88-0fe5-4514-80fd-c0292064d32a";
		private const string COIN_API_KEY = "goldapi-k2derlpzpnqxf-io";
		private readonly HttpClient httpClient;
		

		public ApiService(HttpClient httpClient)
		{
			this.httpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
		}

		public async Task<ConvertModel> Convert(string curr,string symbol,double ammount,string year,string month,string day)
		{
			string apiKey = "bce54fad4f67965a35c9754582779520";
			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			var url = $"https://api.metalpriceapi.com/v1/convert?api_key={apiKey}&from={symbol}&to={curr}&amount={ammount}&date={year}-{month}-{day}";
			
			try
			{
				var response = httpClient.GetAsync(url).Result;

				//response.EnsureSuccessStatusCode();
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
		public async Task<Dictionary<string, string>> GetMetalDict()
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
