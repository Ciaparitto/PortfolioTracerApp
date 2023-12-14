using PortfolioApp.Models;

namespace PortfolioApp.Components.Services.Interfaces
{
	public interface IUserService
	{
	
		public Task<UserModel> GetLoggedUser();
		public  Task<double> GetAmmountOfAsset(string AssetCode, string typeOfAsset);
	}
}
