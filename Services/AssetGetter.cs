using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using System.Net.Http;

namespace PortfolioApp.Services
{
	public class AssetGetter : IAssetGetter
	{
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;
		private readonly HttpClient _HttpClient;
		public AssetGetter(AppDbContext Context, IUserGetter userGetter, HttpClient httpClient)
		{			
			_Context = Context;
			_UserGetter = userGetter;
			_HttpClient = httpClient;
		}

		public async Task<Dictionary<string, double>> GetUserAssets(bool IsTrial)
		{
			var User = await _UserGetter.GetLoggedUser();

			var Dict = new Dictionary<string, double>();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id && x.IsTrialTransaction == IsTrial).OrderBy(x => x.date).ToList();
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
		public async Task<Dictionary<string, double>> GetUserAssetsByType(string Type, bool IsTrial)
		{
			var User = await _UserGetter.GetLoggedUser();

			var Dict = new Dictionary<string, double>();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id && x.TypeOfAsset == Type && x.IsTrialTransaction == IsTrial).OrderBy(x => x.date).ToList();
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
		public async Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset, bool IsTrial)
		{
			var User = await _UserGetter.GetLoggedUser();
			if (User != null)
			{
				var AssetList = _Context.Transactions.Where(x => x.AssetCode == AssetCode && x.TypeOfAsset == typeOfAsset && x.UserId == User.Id && x.IsTrialTransaction == IsTrial).ToList();
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
		public async Task<double> GetAssetValue(string AssetCode, double Ammount, UserModel User)
		{
			HttpResponseMessage Response = _HttpClient.GetAsync($"/Api/GetRatesLasted").Result;
			if (Response.IsSuccessStatusCode)
			{
				string Result = await Response.Content.ReadAsStringAsync();
				var Currency = JsonConvert.DeserializeObject<CurrencyModel>(Result);
				var Rate = 1 / ((double)Currency.Rates[AssetCode]);
				double Value = Rate * Ammount;
				return Value;
			}
			return 0;
		}
	}
}
