using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
    public interface IApiService
    {
      
        public  Task<MetalModel> GetMetalPrice(string symbol, string curr, string year, string month, string day);
        public Task<MetalModel> GetMetalPrice(string symbol, string curr);

		public Task<CurrencyModel> GetCurrencyPrice(string basecurrency, string currencyList, string year, string month, string day);
        
	}
}
