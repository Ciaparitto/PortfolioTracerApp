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

			string apiKey = "4fc262fd8b1aa817e289adbd28a2166b";
			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			string url = $"https://api.metalpriceapi.com/v1/{Date}?api_key={apiKey}";

			try

			{
				var response = httpClient.GetAsync(url).Result;
				string result = await response.Content.ReadAsStringAsync();
				var Metal = JsonConvert.DeserializeObject<CurrencyModel>(result);
				return Metal;
			}
			catch (Exception err)
			{
				Console.WriteLine($"error {err.Message}");
				return null;
			}
		}
	
	

	}
}
