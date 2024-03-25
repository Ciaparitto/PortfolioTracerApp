using Microsoft.AspNetCore.Identity;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class UserGetter : IUserGetter
	{
		private readonly UserManager<UserModel> _UserManager;
		private readonly IHttpContextAccessor _HttpContextAccessor;

		public UserGetter(UserManager<UserModel> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_UserManager = userManager;
			_HttpContextAccessor = httpContextAccessor;
		}
		public async Task<UserModel> GetLoggedUser()
		{
			var User = await _UserManager.GetUserAsync(_HttpContextAccessor.HttpContext.User);
			return User;
		}

	}
}
