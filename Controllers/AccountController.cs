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
		public async Task<IActionResult> AddAssetAsync()
		{

			Dictionary<string, string> CurrenciesDict = new Dictionary<string, string>
			{
			{"USD", "United States Dollar"},
			{"EUR", "Euro"},
			{"GBP", "British Pound Sterling"},
			{"JPY", "Japanese Yen"},
			{"AUD", "Australian Dollar"},
			{"CAD", "Canadian Dollar"},
			{"CHF", "Swiss Franc"},
			{"CNY", "Chinese Yuan"},
			{"SEK", "Swedish Krona"},
			{"NZD", "New Zealand Dollar"},
			{"NOK", "Norwegian Krone"},
			{"DKK", "Danish Krone"},
			{"SGD", "Singapore Dollar"},
			{"HKD", "Hong Kong Dollar"},
			{"KRW", "South Korean Won"},
			{"TRY", "Turkish Lira"},
			{"ZAR", "South African Rand"},
			{"SAR", "Saudi Riyal"},
			{"AED", "United Arab Emirates Dirham"},
			{"QAR", "Qatari Riyal"},
			{"MYR", "Malaysian Ringgit"},
			{"THB", "Thai Baht"},
			{"IDR", "Indonesian Rupiah"},
			{"INR", "Indian Rupee"},
			{"PHP", "Philippine Peso"},
			{"PKR", "Pakistani Rupee"},
			{"KWD", "Kuwaiti Dinar"},
			{"BHD", "Bahraini Dinar"},
			{"OMR", "Omani Rial"},
			{"JOD", "Jordanian Dinar"},
			{"GBP", "British Pound Sterling"},
			{"EUR", "Euro"},
			{"CHF", "Swiss Franc"},
			{"CAD", "Canadian Dollar"},
			{"AUD", "Australian Dollar"},
			{"NZD", "New Zealand Dollar"},
			{"SGD", "Singapore Dollar"},
			{"HKD", "Hong Kong Dollar"},
			{"SEK", "Swedish Krona"},
			{"NOK", "Norwegian Krone"},
			{"DKK", "Danish Krone"},
			{"JPY", "Japanese Yen"},
			{"CNY", "Chinese Yuan"},
			{"KRW", "South Korean Won"},
			{"TRY", "Turkish Lira"},
			{"ZAR", "South African Rand"},
			{"SAR", "Saudi Riyal"},
			{"AED", "United Arab Emirates Dirham"},
			{"QAR", "Qatari Riyal"},
			{"MYR", "Malaysian Ringgit"},
			{"THB", "Thai Baht"},
			{"IDR", "Indonesian Rupiah"},
			{"INR", "Indian Rupee"},
			{"PHP", "Philippine Peso"},
			{"PKR", "Pakistani Rupee"},
			};

			Dictionary<string, string> MetalsDict = new Dictionary<string, string>
			{
			{ "XAU", "Gold" },
			{ "XAG", "Silver" },
			{ "XPT", "Platinum" },
			{ "XPD", "Palladium" },
			};

			Dictionary<string, string> CryptoDict = new Dictionary<string, string>
			{
			{ "BTC", "Bitcoin" },
			{ "ETH", "Ethereum" },
			{ "XRP", "Ripple" },
			{ "LTC", "Litecoin" },
			{ "BCH", "Bitcoin Cash" },
			{ "ADA", "Cardano" },
			{ "DOT", "Polkadot" },
			{ "XLM", "Stellar" },
			{ "DOGE", "Dogecoin" },
			{ "USDT", "Tether" },
			{ "XMR", "Monero" },
			{ "EOS", "EOS.IO" },
			{ "TRX", "TRON" },
			{ "XTZ", "Tezos" },
			{ "DASH", "Dash" },
			{ "ATOM", "Cosmos" },
			{ "LINK", "Chainlink" },
			{ "UNI", "Uniswap" },
			{ "AAVE", "Aave" },
			{ "SNX", "Synthetix" },
			};
			ViewBag.Crypto = CryptoDict;
			ViewBag.Metals = MetalsDict;
			ViewBag.Currencies = CurrenciesDict;

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
		public async Task<Dictionary<string,double>> GetUserAssets()
		{
			var User = await GetLoggedUser();
			var Dict = new Dictionary<string,double>();
			if (User != null)
			{
				var AssetList = _Context.Assets.Where(x => x.UserId == User.Id).ToList();
				foreach (var Asset in AssetList)
				{
					Dict[Asset.AssetCode] = Asset.Ammount;
				}				
			}
			return Dict;
		}
	}
}
