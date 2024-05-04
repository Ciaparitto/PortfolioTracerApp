using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using System.Configuration;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class ApiController : Controller
	{
		private readonly HttpClient HttpClient;
		private readonly string _ApiKey;

		public ApiController(HttpClient httpClient, IConfiguration Configuration)
		{
			this.HttpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
			_ApiKey = Configuration.GetValue<string>("AppSettings:ApiKey");
		}
		public async Task<CurrencyModel> GetRatesByDay(string Date)
		{
			HttpClient.DefaultRequestHeaders.Add("x-access-token", _ApiKey);
			string url = $"https://api.metalpriceapi.com/v1/{Date}?api_key={_ApiKey}";

			try
			{
				var Response = HttpClient.GetAsync(url).Result;
				string Result = await Response.Content.ReadAsStringAsync();
				var Currency = JsonConvert.DeserializeObject<CurrencyModel>(Result);
				return Currency;
			}
			catch (Exception Error)
			{
				Console.WriteLine($"error {Error.Message}");
				return null;
			}
		}

		public async Task<CurrencyModel> GetRatesLasted()
		{

			HttpClient.DefaultRequestHeaders.Add("x-access-token", _ApiKey);
			string url = $"https://api.metalpriceapi.com/v1/latest?api_key={_ApiKey}";

			try
			{
				var Response = HttpClient.GetAsync(url).Result;
				string Result = await Response.Content.ReadAsStringAsync();
				var Currency = JsonConvert.DeserializeObject<CurrencyModel>(Result);
				return Currency;
			}
			catch (Exception Error)
			{
				Console.WriteLine($"error {Error.Message}");
				return null;
			}
		}
	}
}
