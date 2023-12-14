using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Models;

namespace PortfolioApp.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<UserModel> _userManager;
		private readonly SignInManager<UserModel> _signInManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
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
		

	}
}
