using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class ApiController : Controller
	{
		private readonly HttpClient HttpClient;
		private readonly string _ApiKey;
		private readonly ISecretsGetter _SecretGetter;
		public ApiController(HttpClient httpClient,ISecretsGetter SecretsGetter)
		{
			this.HttpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
			_SecretGetter = SecretsGetter;
			//_ApiKey = "f57e62e39a31c96da58a32fdd1744049";
			_ApiKey = _SecretGetter.GetSecret("ApiKey");
			Console.WriteLine(_ApiKey);
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
