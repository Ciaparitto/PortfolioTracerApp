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
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;

		public UserService(IUserGetter UserGetter, UserManager<UserModel> userManager, AppDbContext appDbContext)
		{
			_userManager = userManager;
			_Context = appDbContext;
			_UserGetter = UserGetter;
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

			var User = await _UserGetter.GetLoggedUser();
			if (User != null)
			{

				if (await _userManager.CheckPasswordAsync(User, currentPassword))
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
