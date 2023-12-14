﻿using System.ComponentModel.DataAnnotations;

namespace PortfolioApp.Models
{
	public class RegisterModel
	{
		[Required]
		public string EmailAdress { get; set; }
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	
	}
}
