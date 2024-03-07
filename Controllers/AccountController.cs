using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<UserModel> _UserManager;
		private readonly SignInManager<UserModel> _SignInManager;
		private readonly IDbService _DbService;
		public readonly IUserService _UserService;
		private readonly HttpClient HttpClient;
		private readonly AppDbContext _Context;
		public AccountController(HttpClient httpClient, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IDbService dbService, IUserService userService, AppDbContext context)
		{
			_UserManager = userManager;
			_SignInManager = signInManager;
			_DbService = dbService;
			_UserService = userService;
			this.HttpClient = httpClient;
			_Context = context;
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel RegisterData)
		{


			if (ModelState.IsValid)
			{
				var NewUser = new UserModel
				{
					Email = RegisterData.EmailAdress,
					UserName = RegisterData.UserName,
				};
				await _UserManager.CreateAsync(NewUser, RegisterData.Password);
				await _SignInManager.PasswordSignInAsync(RegisterData.Password, RegisterData.Password, false, false);
				return Redirect("https://localhost:7080/");
			}
			return View(RegisterData);
		}
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel LoginData)
		{
			if (ModelState.IsValid)
			{
				await _SignInManager.PasswordSignInAsync(LoginData.UserName, LoginData.Password, false, false);
				return Redirect("/");
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _SignInManager.SignOutAsync();
			return Redirect("/");

		}
		[HttpPost]
		public async Task<IActionResult> AddAsset(AssetModel AssetBody)
		{

			if (ModelState.IsValid)
			{
				var Model = new AssetModel
				{
					AssetCode = AssetBody.AssetCode,
					Ammount = AssetBody.Ammount,
					TypeOfAsset = AssetBody.TypeOfAsset,
					UserId = _UserService.GetLoggedUser().Result.Id,
				};
				await _DbService.AddAssetToDb(Model);
				return Redirect("/");
			}
			return View(AssetBody);
		}
		[HttpGet]
		[Route("/YourAccount")]
		public async Task<IActionResult> Account()
		{
			var User = await GetLoggedUser();
			ViewBag.UserName = User.UserName;

			return View();
		}
		public async Task<UserModel> GetLoggedUser()
		{
			var User = await _UserManager.GetUserAsync(base.User);
			return User;
		}
		public async Task<double> GetAmmountOfAsset(string AssetCode)
		{
			var User = await GetLoggedUser();
			if (User != null)
			{

				var AssetList = _Context.Assets.Where(x => x.AssetCode == AssetCode && x.UserId == User.Id).ToList();
				double Ammount = 0;
				foreach (var Asset in AssetList)
				{
					Ammount += Asset.Ammount;
				}
				return Ammount;
			}
			return 0;
		}
		public async Task<Dictionary<string, double>> GetUserAssets()
		{

			var User = await GetLoggedUser();


			var Dict = new Dictionary<string, double>();
			if (User != null)
			{

				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id).ToList();
				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode) || Dict[Asset.AssetCode] == null)
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
				var AssetList = _Context.Transactions.Where(x => x.UserId == User.Id && x.TypeOfAsset == Type).ToList();

				foreach (var Asset in AssetList)
				{
					if (!Dict.ContainsKey(Asset.AssetCode) || Dict[Asset.AssetCode] == null)
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
		public async Task<bool> CheckPassword(string Password)
		{
			var User = await GetLoggedUser();
			var PasswordCheck = await _UserManager.CheckPasswordAsync(User, Password);
			{
				if (PasswordCheck)
				{
					return true;
				}
			}
			return false;
		}
		public async Task ChangePassword(string CurrentPassword, string NewPassword)
		{
			var User = await GetLoggedUser();
			if (User != null)
			{
				var Result = await _UserManager.ChangePasswordAsync(User, CurrentPassword, NewPassword);
				if (Result.Succeeded)
				{
					await _Context.SaveChangesAsync();
				}
			}
		}

		public async Task ChangeUsername(string CurrentPassword, string NewUser)
		{
			var User = GetLoggedUser().Result;
			if (User != null)
			{
				var PasswordCheck = await _UserManager.CheckPasswordAsync(User, CurrentPassword);
				{
					if (PasswordCheck)
					{
						var Result = await _UserManager.SetUserNameAsync(User, NewUser);
						if (Result.Succeeded)
						{
							await _Context.SaveChangesAsync();
						}

					}
				}
			}
		}
	}
}

