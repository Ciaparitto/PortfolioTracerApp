using Microsoft.AspNetCore.Mvc;

namespace PortfolioApp.Controllers
{
	public class AssetController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
