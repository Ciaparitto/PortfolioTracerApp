using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IDbHelper
	{
		public Task AddTransactionToDb(TransactionModel model);
	}
}
