using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Models;
using PortfolioApp.Services;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Controllers
{
	public class AssetController : Controller
	{

		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;
		public AssetController(AppDbContext context,IUserGetter userGetter)
		{
			_Context = context;
			_UserGetter = userGetter;
		}

		public async Task<Dictionary<string, double>> GetUserAssets()
		{

			var User = await _UserGetter.GetLoggedUser();


			var Dict = new Dictionary<string, double>();
			if (User != null)
			{

				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id).ToList();
				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode) || Dict[Asset.AssetCode] == null)
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
			var User = await _UserGetter.GetLoggedUser();

			var Dict = new Dictionary<string, double>();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id && x.TypeOfAsset == Type).ToList();

				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode) || Dict[Asset.AssetCode] == null)
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
	}
}
