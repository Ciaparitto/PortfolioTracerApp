using PortfolioApp.Models;

namespace PortfolioApp.Services.Interfaces
{
	public interface IUserGetter
	{
		public Task<UserModel> GetLoggedUser();
	}
}
