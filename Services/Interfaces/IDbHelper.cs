using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IDbHelper
	{
		public Task AddAssetToDb(AssetModel body);
	
		public Task AddTransactionToDb(TransactionModel model);
	}
}
