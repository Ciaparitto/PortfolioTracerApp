using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using System.Net.Http;

namespace PortfolioApp.Services
{
	public class AssetGetter : IAssetGetter
	{
		private readonly IUserService _UserService;
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;
		private readonly HttpClient _HttpClient;
		public AssetGetter(IUserService UserService, AppDbContext Context, IUserGetter userGetter, HttpClient httpClient)
		{
			_UserService = UserService;
			_Context = Context;
			_UserGetter = userGetter;
			_HttpClient = httpClient;
		}

		public async Task<Dictionary<string, double>> GetUserAssets()
		{
			var User = await _UserGetter.GetLoggedUser();

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
			var User = await _UserGetter.GetLoggedUser();

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
			var User = await _UserGetter.GetLoggedUser();
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
		public Task<double> GetAssetsValue(string AssetCode, double Ammount, UserModel User)
		{
			DateTime CurrentDate = DateTime.Now;
			string FormattedDate = CurrentDate.ToString("dd-MM-yyyy");
			HttpResponseMessage Response = _HttpClient.GetAsync($"/Api/GetRatesByDay?Date={FormattedDate}").Result;
			if (Response.IsSuccessStatusCode)
			{

			}

			return null;
		}
	}
}
