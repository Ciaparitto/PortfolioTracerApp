using PortfolioApp.Components.Services.Interfaces;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using PortfolioApp.Models;


namespace PortfolioApp.Components.Services
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

		public async Task<string> GetMetalPrice(string symbol,string curr,string year,string month,string day)
		{
		
				string apiKey = "goldapi-k2derlpzpnqxf-io";
				string date = year + month + day;

				
				httpClient.DefaultRequestHeaders.Add("x-access-token", apiKey);				
				string url = $"https://www.goldapi.io/api/{symbol}/{curr}/{date}";
				try

				{					
					var response = httpClient.GetAsync(url).Result;
					
					//response.EnsureSuccessStatusCode();
					string result = await response.Content.ReadAsStringAsync();
					var Metal = JsonConvert.DeserializeObject<MetalModel>(result);
					var result2 = $"cena za {Metal.metal} to {Math.Round(Metal.price)} {curr} w dniu {year}.{month}.{day}";
					return result2.ToString();

				}
				catch (Exception err)
				{
					Console.WriteLine($"blad {err.Message}");
					return $"blad {err.Message}";
				}		
		}
		public async Task<string> GetCurrencyPrice(string basecurrency,string currencyList,string year, string month, string day)
		{
			string ApiKey = "4b6fd8ef52c20240e71494066ee0354d";
			string url = $"http://api.exchangeratesapi.io/v1/{year}-{month}-{day}?access_key={ApiKey}&{basecurrency}&{currencyList}";
			try
			{
				var response = httpClient.GetAsync(url).Result;
				string result = await response.Content.ReadAsStringAsync();
				var Currency = JsonConvert.DeserializeObject<CurrencyModel>(result);
				return Currency.Base;
				Console.WriteLine("wait");
			}
			catch (Exception err)
			{
				Console.WriteLine(err.Message);
				return err.Message;
			}
			
		}
		
	}
}
