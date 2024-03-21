using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Controllers
{
	public class DataBaseController : Controller
	{
		private readonly IUserService _UserService;
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;
		public DataBaseController(IUserService userService, AppDbContext context,IUserGetter userGetter)
		{

			_UserService = userService;
			_Context = context;
			_UserGetter = userGetter;
		}

		public async Task<double> GetAmmountOfAsset(string AssetCode)
		{
			var User = await _UserGetter.GetLoggedUser();
			if (User != null)
			{

				var AssetList = _Context.Assets.Where(x => x.AssetCode == AssetCode && x.UserId == User.Id).ToList();
				double Ammount = 0;
				foreach (var Asset in AssetList)
				{
					Ammount += Asset.Ammount;
				}
				return Ammount;
			}
			return 0;
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
