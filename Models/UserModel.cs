using Microsoft.AspNetCore.Identity;

namespace PortfolioApp.Models
{
	public class UserModel : IdentityUser
	{
		public double Money { get; set; }

	}
}
