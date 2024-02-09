using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;

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
			
				var User = await GetLoggedUser();

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
		public async Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset)
		{
			var User = await GetLoggedUser();
			if (User != null)
			{

				var AssetList = _Context.Transactions.Where(x => x.AssetCode == AssetCode && x.TypeOfAsset == typeOfAsset && x.UserId == User.Id).ToList();
				double Ammount = 0;
				foreach (var Asset in AssetList)
				{
					if(Asset.TransactionType == "Deposit")
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
        public async Task<bool> CheckPassword(string password)
        {
            var USER = await GetLoggedUser();
            var passwordCheck = await _userManager.CheckPasswordAsync(USER, password);
            {
                if (passwordCheck)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task ChangePassword(string currentPassword, string newPassword)
        {
            var USER = await GetLoggedUser();
            if (USER != null)
            {         
                var result = await _userManager.ChangePasswordAsync(USER, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    await _Context.SaveChangesAsync();              
                }
            }
        }
     
        public async Task ChangeUsername(string currentPassword, string newUsername)

        {

            var USER = GetLoggedUser().Result;
            if (USER != null)
            {               
                var passwordCheck = await _userManager.CheckPasswordAsync(USER, currentPassword);
                {
                    if (passwordCheck)
                    {
                        var result = await _userManager.SetUserNameAsync(USER, newUsername);
                        if (result.Succeeded)
                        {
                            await _Context.SaveChangesAsync();                        
                        }

                    }
                }


            }
       
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
         
        }
    }
}
