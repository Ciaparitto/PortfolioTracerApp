using Microsoft.AspNetCore.Identity;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;

namespace PortfolioApp.Components.Services
{
	public class UserService : IUserService
	{

		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AppDbContext _Context;

		public UserService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
			_Context = appDbContext;
		}

		public async Task<UserModel> GetLoggedUser()
		{
			var _User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
			return _User;
		}

		public async Task<double> GetAmmountOfAsset(string AssetCode,string typeOfAsset)
		{
			var Userid = GetLoggedUser().Id;
			var AssetList = _Context.Assets.Where(x => x.AssetCode == AssetCode && x.TypeOfAsset == typeOfAsset).ToList();
			double Ammount = 0;
			foreach (var Asset in AssetList)
			{
				Ammount += Asset.Ammount;
			}
			return Ammount;
		}
	}
}
