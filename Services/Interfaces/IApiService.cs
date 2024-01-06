using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
    public interface IApiService
    {
     
        public Task<ConvertModel> Convert(string curr, string symbol, double ammount, string year, string month, string day);
        public  Task<Dictionary<string, string>> GetMetalDict();
		public Task<Dictionary<string, string>> GetCurrencyDict();
		public Task<Dictionary<string, string>> GetCryptoCurrencyDict();
	}
}
