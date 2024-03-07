using Microsoft.AspNetCore.Mvc;

namespace PortfolioApp.Services.Interfaces
{
	public interface IAssetGetter
	{
		public Task<Dictionary<string, double>> GetUserAssetsByType(string Type);
		public Task<Dictionary<string, double>> GetUserAssets();
		public Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset);
	}
}
