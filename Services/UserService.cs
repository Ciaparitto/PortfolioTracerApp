using Microsoft.AspNetCore.Identity;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using System.Security.Claims;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using static System.Net.WebRequestMethods;
using System.ComponentModel.DataAnnotations;
namespace PortfolioApp.Components.Services
{
	public class UserService : IUserService
	{
		private readonly HttpClient httpClient;
		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AppDbContext _Context;

		public UserService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext, HttpClient httpClient)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
			_Context = appDbContext;
			this.httpClient = httpClient;
		}
		public async Task<UserModel> GetLoggedUser()
		{
			var _User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
			if (_User != null)
			{
				return _User;
			}
			return null;
		}

		public async Task<Dictionary<string, double>> GetUserAssets()
		{
			
			var User = await GetLoggedUser();


			var Dict = new Dictionary<string, double>();
				if (User != null)
				{
					
					var AssetList = _Context.Assets.Where(x => x.UserId == User.Id).ToList();
					foreach (var Asset in AssetList)
					{
						Dict[Asset.AssetCode] = Asset.Ammount;
					}
				}
			return Dict;
		
			
		}
		public async Task<Dictionary<string, double>> GetUserAssetsByType(string Type)
		{
			
				var User = await GetLoggedUser();

				var Dict = new Dictionary<string, double>();
				if (User != null)
				{
					var AssetList = _Context.Assets.Where(x => x.UserId == User.Id && x.TypeOfAsset == Type).ToList();
					
					foreach (var Asset in AssetList)
					{
						Dict[Asset.AssetCode] = Asset.Ammount;
					}
				}
				return Dict;
			
		}
		public async Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset)
		{
			var User = GetLoggedUser().Result;
			if (User != null)
			{

				var AssetList = _Context.Assets.Where(x => x.AssetCode == AssetCode && x.TypeOfAsset == typeOfAsset && x.UserId == User.Id).ToList();
				double Ammount = 0;
				foreach (var Asset in AssetList)
				{
					Ammount += Asset.Ammount;
				}
				return Ammount;
			}
			return 0;
		}
	}
}
