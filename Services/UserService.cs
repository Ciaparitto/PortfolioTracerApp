using Microsoft.AspNetCore.Identity;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using System.Security.Claims;

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
			if (_User != null)
			{
				return _User;
			}
			return null;
		}

		public async Task<double> GetAmmountOfAsset(string AssetCode,string typeOfAsset)
		{
			var User = GetLoggedUser().Result;
			if(User!= null)
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
