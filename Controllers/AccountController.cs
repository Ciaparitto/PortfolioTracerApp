using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services;
using PortfolioApp.Services.Interfaces;
using System.Net.Http;

namespace PortfolioApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<UserModel> _UserManager;
		private readonly SignInManager<UserModel> _SignInManager;
		private readonly IDbHelper _DbService;
		private readonly IUserService _UserService;
		private readonly HttpClient HttpClient;
		private readonly AppDbContext _Context;
		private readonly IUserGetter _UserGetter;
		public AccountController(IUserGetter UserGetter, HttpClient httpClient, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IDbHelper dbService, IUserService userService, AppDbContext context)
		{
			_UserManager = userManager;
			_SignInManager = signInManager;
			_DbService = dbService;
			_UserService = userService;
			this.HttpClient = httpClient;
			_Context = context;
			_UserGetter = UserGetter;
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
				if (_Context.Users.Where(User => User.Email == RegisterData.EmailAdress).ToList().Count != 0)
				{
					ViewBag.Error = $"Email {RegisterData.EmailAdress} is already taken";
					return View();
				}
				var Result = await _UserManager.CreateAsync(NewUser, RegisterData.Password);

				if (Result.Succeeded)
				{
					await _SignInManager.PasswordSignInAsync(RegisterData.Password, RegisterData.Password, false, false);
					return Redirect("/");
				}
				else
				{
					ViewBag.Error = Result.Errors.FirstOrDefault().Description;
				}
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
				var Result = await _SignInManager.PasswordSignInAsync(LoginData.UserName, LoginData.Password, false, false);
				if (Result.Succeeded)
				{
					return Redirect("/");
				}
				else
				{
					ViewBag.Error = "Username or password is wrong";
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _SignInManager.SignOutAsync();
			return Redirect("/");

		}
		[HttpGet]
		[Route("/YourAccount")]
		public async Task<IActionResult> Account()
		{
			var User = await _UserGetter.GetLoggedUser();
			ViewBag.UserName = User.UserName;

			return View();
		}
		public async Task<bool> CheckPassword(string Password)
		{
			var User = await _UserGetter.GetLoggedUser();
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
			var User = await _UserGetter.GetLoggedUser();
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
			var User = _UserGetter.GetLoggedUser().Result;
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

