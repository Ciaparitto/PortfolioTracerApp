using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class DbHelper : IDbHelper
	{
		private readonly AppDbContext _Context;
		public DbHelper(AppDbContext context)
		{
			_Context = context;
		}
		public async Task AddAssetToDb(AssetModel model)
		{
			await _Context.Assets.AddAsync(model);
			await _Context.SaveChangesAsync();
		}

		public async Task AddTransactionToDb(TransactionModel model)
		{
			await _Context.Transactions.AddAsync(model);
			await _Context.SaveChangesAsync();
		}
	}
}
