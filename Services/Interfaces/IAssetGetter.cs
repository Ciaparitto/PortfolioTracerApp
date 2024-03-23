using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IAssetGetter
	{
		public Task<Dictionary<string, double>> GetUserAssetsByType(string Type);
		public Task<Dictionary<string, double>> GetUserAssets(bool IsTrial);
		public Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset, bool IsTrial);
		public Task<double> GetAssetsValue(string AssetCode, double Ammount, UserModel User);

	}
}
