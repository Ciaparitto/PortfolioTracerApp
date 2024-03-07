using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class AssetGetter : IAssetGetter
	{
		private readonly IUserService _UserService;
		private readonly AppDbContext _Context;
		public AssetGetter(IUserService UserService, AppDbContext Context)
		{
			_UserService = UserService;
			_Context = Context;
		}

		public async Task<Dictionary<string, double>> GetUserAssets()
		{
			var User = await _UserService.GetLoggedUser();

			var Dict = new Dictionary<string, double>();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id).OrderBy(x => x.date).ToList();
				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode))
					{
						Dict[Asset.AssetCode] = Asset.Ammount;
					}
					else
					{
						if (Asset.TransactionType == "Deposit")
						{
							Dict[Asset.AssetCode] += Asset.Ammount;
						}
						else
						{
							Dict[Asset.AssetCode] -= Asset.Ammount;
						}
					}
					if (Dict[Asset.AssetCode] <= 0)
					{
						Dict.Remove(Asset.AssetCode);
					}
				}
			}
			return Dict;
		}
		public async Task<Dictionary<string, double>> GetUserAssetsByType(string Type)
		{
			var User = await _UserService.GetLoggedUser();

			var Dict = new Dictionary<string, double>();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id && x.TypeOfAsset == Type).OrderBy(x => x.date).ToList();
				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode))
					{
						Dict[Asset.AssetCode] = Asset.Ammount;
					}
					else
					{
						if (Asset.TransactionType == "Deposit")
						{
							Dict[Asset.AssetCode] += Asset.Ammount;
						}
						else
						{
							Dict[Asset.AssetCode] -= Asset.Ammount;
						}
					}
					if (Dict[Asset.AssetCode] <= 0)
					{
						Dict.Remove(Asset.AssetCode);
					}
				}
			}
			return Dict;
		}
		public async Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset)
		{
			var User = await _UserService.GetLoggedUser();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.AssetCode == AssetCode && x.TypeOfAsset == typeOfAsset && x.UserId == User.Id).ToList();
				double Ammount = 0;
				foreach (var Asset in AssetList)
				{
					if (Asset.TransactionType == "Deposit")
					{
						Ammount += Asset.Ammount;
					}
					else
					{
						Ammount -= Asset.Ammount;
					}
				}
				return Ammount;
			}
			return 0;
		}
	}
}
