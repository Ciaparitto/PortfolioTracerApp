using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class DbService : IDbService
	{
		private readonly AppDbContext _Context;
		public DbService(AppDbContext context) 
		{
			_Context = context;
		}
		public async Task AddAssetToDb(AssetModel model)
		{
			await _Context.Assets.AddAsync(model);
			await _Context.SaveChangesAsync();
		}
		

	}
}
