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

		public async Task<MetalModel> GetMetalPrice(string symbol,string curr,string year,string month,string day)
		{
		
				string apiKey = "goldapi-5b18drlq3tux1b-io";
				string date = year + month + day;

				
				httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
				string url = $"https://www.goldapi.io/api/{symbol}/{curr}/{date}";
				try

				{					
					var response = httpClient.GetAsync(url).Result;
					
					//response.EnsureSuccessStatusCode();
					string result = await response.Content.ReadAsStringAsync();
					var Metal = JsonConvert.DeserializeObject<MetalModel>(result);
					var Result = Math.Round(Metal.price ,2);
					return Metal;
	
				}
				catch (Exception err)
				{
					Console.WriteLine($"blad {err.Message}");
					return null;
				}		
		}
		public async Task<MetalModel> GetMetalPrice(string symbol, string curr)
		{

			string apiKey = "goldapi-5b18drlq3tux1b-io";
			


			httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);
			string url = $"https://www.goldapi.io/api/{symbol}/{curr}";
			try

			{
				var response = httpClient.GetAsync(url).Result;

				//response.EnsureSuccessStatusCode();
				string result = await response.Content.ReadAsStringAsync();
				var Metal = JsonConvert.DeserializeObject<MetalModel>(result);
				var Result = Math.Round(Metal.price, 2);
				return Metal;

			}
			catch (Exception err)
			{
				Console.WriteLine($"blad {err.Message}");
				return null;
			}
		}
		public async Task<CurrencyModel> GetCurrencyPrice(string basecurrency,string currencyList,string year, string month, string day)
		{
			string ApiKey = "4b6fd8ef52c20240e71494066ee0354d";
			string url = $"http://api.exchangeratesapi.io/v1/{year}-{month}-{day}?access_key={ApiKey}&{basecurrency}&{currencyList}";
			try
			{
				var response = httpClient.GetAsync(url).Result;
				string result = await response.Content.ReadAsStringAsync();
				var Currency = JsonConvert.DeserializeObject<CurrencyModel>(result);
				return Currency;
			}
			catch (Exception err)
			{
				Console.WriteLine(err.Message);
				return null;
			}
			
		}
		
	}
}
