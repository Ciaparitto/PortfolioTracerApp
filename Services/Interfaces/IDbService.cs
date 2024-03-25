using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IDbService
	{
		public Task AddAssetToDb(AssetModel body);
		public Task<Dictionary<string, string>> GetMetalDict();
		public Task<Dictionary<string, string>> GetCurrencyDict();
		public Task<Dictionary<string, string>> GetCryptoCurrencyDict();
		public Task AddTransactionToDb(TransactionModel model);
	}
}
