using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class DbService : IDbService
	{
		private readonly AppDbContext _Context;
		private readonly IUserService _userService;
		public DbService(AppDbContext context,IUserService userService) 
		{
			_Context = context;
			_userService = userService;
		}
		public async Task AddAssetToDb(AssetModel model)
		{
			await _Context.Assets.AddAsync(model);
			await _Context.SaveChangesAsync();
		}
		public async Task<List<AssetModel>> GetAssetList(string typeOfAsset)
		{
			var user = await  _userService.GetLoggedUser();
			
			var assetList = await _Context.Assets.Where(x => x.TypeOfAsset == typeOfAsset && x.UserId == user.Id).ToListAsync();

			return assetList;
		}
		public async Task<List<AssetModel>> GetAssetList()
		{
			var user = await _userService.GetLoggedUser();

			var assetList = await _Context.Assets.Where(x => x.UserId == user.Id).ToListAsync();

			return assetList;
		}

	}
}
