using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;
		private readonly IDbService _DbService;
		public readonly IUserService _userService;
		

		public UserController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IDbService dbService, IUserService userService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_DbService = dbService;
			_userService = userService;
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

			var NewUser = new UserModel
			{
				Email = data.EmailAdress,
				UserName = data.UserName,
			};
			if (ModelState.IsValid)
			{
				await _userManager.CreateAsync(NewUser, data.Password);
				await _signInManager.PasswordSignInAsync(data.Password, data.Password, false, false);
				return Redirect("https://localhost:7080/");
			}
			return View(NewUser);
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
		[HttpGet]
		public IActionResult AddAsset()
		{
			return View();
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

	}
}
