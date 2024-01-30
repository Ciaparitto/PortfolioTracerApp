using PortfolioApp.Models;

namespace PortfolioApp.Components.Services.Interfaces
{
	public interface IUserService
	{

		public Task<UserModel> GetLoggedUser();
		public Task<Dictionary<string, double>> GetUserAssetsByType(string Type);
		public Task<Dictionary<string, double>> GetUserAssets();
		public Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset);
	
	}
}
