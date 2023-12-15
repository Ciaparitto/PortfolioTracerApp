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

		public async Task<CurrencyModel> GetMetalPrice(string symbol,string curr,string year,string month,string day)
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
	}
}
