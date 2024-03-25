using Microsoft.EntityFrameworkCore;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class TransactionGetter : ITransactionGetter
	{
		private readonly IUserGetter _UserGetter;
		private readonly AppDbContext _Context;
		public TransactionGetter(IUserGetter UserGetter, AppDbContext Context)
		{
			_UserGetter = UserGetter;
			_Context = Context;
		}
		public async Task<List<TransactionModel>> GetUserTransactions(bool IsTrialTransaction)
		{
			var USER = await _UserGetter.GetLoggedUser();
			var List = await _Context.Transactions.Where(x => x.UserId == USER.Id && x.IsTrialTransaction == IsTrialTransaction).OrderByDescending(x => x.date).ToListAsync();
			return List;
		}
	}
}
