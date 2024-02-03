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
		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;
		private readonly IDbService _DbService;
		public readonly IUserService _userService;
		private readonly HttpClient httpClient;
		private readonly AppDbContext _Context;
		public AccountController(HttpClient httpClient,UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IDbService dbService, IUserService userService, AppDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_DbService = dbService;
			_userService = userService;
			this.httpClient = httpClient;
			_Context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel data)
		{

          
            if (ModelState.IsValid)
			{
                var NewUser = new UserModel
                {
                    Email = data.EmailAdress,
                    UserName = data.UserName,
                };
                await _userManager.CreateAsync(NewUser, data.Password);
				await _signInManager.PasswordSignInAsync(data.Password, data.Password, false, false);
				return Redirect("https://localhost:7080/");
			}
			return View(data);
		}
		[HttpGet]
		public ActionResult Login()
		{
			return View("Login");
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel data)
		{
			if (ModelState.IsValid)
			{
				await _signInManager.PasswordSignInAsync(data.UserName, data.Password, false, false);
				return Redirect("https://localhost:7080/");
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("https://localhost:7080/");

		}
		[HttpPost]
		public async Task<IActionResult> AddAsset(AssetModel body)
		{
			
			if(ModelState.IsValid)
			{
				var Model = new AssetModel
				{
					AssetCode = body.AssetCode,
					Ammount = body.Ammount,
					TypeOfAsset = body.TypeOfAsset,
					UserId =  _userService.GetLoggedUser().Result.Id,
				};
				await _DbService.AddAssetToDb(Model);
				return Redirect("/");
			}
			return View(body);
		}
		public async Task<UserModel> GetLoggedUser()
		{
			var USER = await _userManager.GetUserAsync(User);
			return USER;
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
		[HttpPost]
		public async Task<IActionResult> ChangePassword(string currentPassword, string password)
		{
		
			var USER = GetLoggedUser().Result;
			if (User.Identity.IsAuthenticated)
			{
				var result = await _userManager.ChangePasswordAsync(USER, currentPassword, password);
				if (result.Succeeded)
				{
					await _Context.SaveChangesAsync();
					await Logout();
				}
				else
				{
					TempData["ErrorMessage"] = "You entered the wrong password";
				}
			}
			return Redirect("/YourAccount");			
		}
		[HttpPost]
		public async Task<IActionResult> ChangeUsername(string currentPassword, string newUsername)
		{

			
			if (User.Identity.IsAuthenticated)
			{
				var USER = GetLoggedUser().Result;
				var passwordCheck = await _userManager.CheckPasswordAsync(USER, currentPassword);
				{
					if(passwordCheck)
					{
						var result = await _userManager.SetUserNameAsync(USER, newUsername);
						if(result.Succeeded)
						{
							await _Context.SaveChangesAsync();
							return Redirect("/");
						}
					
					}
				}
				

			}
			return Redirect("/YourAccount");
		}
		[HttpGet]
		[Route("/Account")]
		public async Task<IActionResult> Account()
		{
			var USER = GetLoggedUser().Result;
			ViewBag.UserName = USER.UserName;
			return View();
		}
	}
}
