using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Models;

namespace PortfolioApp.Components.Services.Interfaces
{
	public interface IUserService
	{



		public Task ChangePassword(string currentPassword, string newPassword);

		public Task ChangeUsername(string currentPassword, string newUsername);
		public Task<bool> CheckPassword(string password);
	}
}
