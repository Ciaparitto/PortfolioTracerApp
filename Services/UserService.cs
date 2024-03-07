using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;

namespace PortfolioApp.Components.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<UserModel> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AppDbContext _Context;

		public UserService(UserManager<UserModel> userManager, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
		{
			_userManager = userManager;
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
	}
}
