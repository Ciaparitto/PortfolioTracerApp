using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
    public interface IApiService
    {
      
        public  Task<CurrencyModel> GetMetalPrice(string symbol, string curr, string year, string month, string day);
        public Task<CurrencyModel> GetMetalPrice(string symbol, string curr);

		public Task<CurrencyModel> GetCurrencyPrice(string basecurrency, string currencyList, string year, string month, string day);
        public Task<ConvertModel> Convert(string curr, string symbol, double ammount, string year, string month, string day);
	}
}
