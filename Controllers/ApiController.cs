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
		private readonly HttpClient httpClient;
		private readonly IUserService _userService;
		private readonly AppDbContext _Context;


		public ApiController(HttpClient httpClient, IUserService userService,AppDbContext context)
		{
			this.httpClient = httpClient;
			httpClient.Timeout = TimeSpan.FromSeconds(30);
			_userService = userService;
			_Context = context;
		}
		public IActionResult Index()

		{
			return View();
		}
		public async Task<CurrencyModel> GetRatesByDay(string Date)
		{

			string apiKey = "8c3ae037a96cb15b6d827fbd139821f2";
			string date = "2023-05-05";
			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			string url = $"https://api.metalpriceapi.com/v1/{Date}?api_key={apiKey}";

	

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
		[IgnoreAntiforgeryToken]
		public async Task AddAssetToDb(AssetModel model)
		{
			await _Context.Assets.AddAsync(model);
			await _Context.SaveChangesAsync();
		}

	}
}
