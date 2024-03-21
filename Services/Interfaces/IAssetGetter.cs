using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IAssetGetter
	{
		public Task<Dictionary<string, double>> GetUserAssetsByType(string Type);
		public Task<Dictionary<string, double>> GetUserAssets();
		public Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset);
		public Task<double> GetAssetsValue(string AssetCode, double Ammount, UserModel User);

	}
}
