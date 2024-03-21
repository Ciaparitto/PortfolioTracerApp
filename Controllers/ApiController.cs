using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class ApiController : Controller
	{
		private readonly HttpClient HttpClient;

		public ApiController(HttpClient httpClient)
		{
			this.HttpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
		}
		public async Task<CurrencyModel> GetRatesByDay(string Date)
		{

			string ApiKey = "4fc262fd8b1aa817e289adbd28a2166b";
			HttpClient.DefaultRequestHeaders.Add("x-access-token", ApiKey);
			string url = $"https://api.metalpriceapi.com/v1/{Date}?api_key={ApiKey}";

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
