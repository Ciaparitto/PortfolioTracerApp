using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface ITransactionGetter
	{
		public Task<List<TransactionModel>> GetUserTransactions(bool IsTrialTransaction);
	}
}
