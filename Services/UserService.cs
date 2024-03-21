using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Components.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<UserModel> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;

		public UserService(IUserGetter UserGetter, UserManager<UserModel> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_Context = appDbContext;
			_UserGetter = UserGetter;
		}


		public async Task<bool> CheckPassword(string password)
		{
			var User = await _UserGetter.GetLoggedUser();
			var passwordCheck = await _userManager.CheckPasswordAsync(User, password);
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
			var User = await _UserGetter.GetLoggedUser();
			if (User != null)
			{
				var result = await _userManager.ChangePasswordAsync(User, currentPassword, newPassword);
				if (result.Succeeded)
				{
					await _Context.SaveChangesAsync();
				}
			}
		}

		public async Task ChangeUsername(string currentPassword, string newUsername)

		{

			var User = _UserGetter.GetLoggedUser().Result;
			if (User != null)
			{
				var passwordCheck = await _userManager.CheckPasswordAsync(User, currentPassword);
				{
					if (passwordCheck)
					{
						var result = await _userManager.SetUserNameAsync(User, newUsername);
						if (result.Succeeded)
						{
							await _Context.SaveChangesAsync();
						}

					}
				}


			}

		}
	}
}
