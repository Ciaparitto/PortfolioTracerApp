using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IDbService
	{
		public Task AddAssetToDb(AssetModel body);
	}
}
