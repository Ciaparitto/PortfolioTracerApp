namespace PortfolioApp.Components.Services.Interfaces
{
    public interface IApiService
    {
      
        public  Task<string> GetMetalPrice(string symbol, string curr, string year, string month, string day);
        public Task<string> GetCurrencyPrice(string basecurrency, string currencyList, string year, string month, string day);

	}
}
