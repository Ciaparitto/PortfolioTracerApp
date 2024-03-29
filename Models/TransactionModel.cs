﻿using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Models
{
	public class TransactionModel
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string TransactionType { get; set; }
		public DateTime date { get; set; } = DateTime.Now;
		public string TypeOfAsset { get; set; }
		public string AssetCode { get; set; }
		public double Ammount { get; set; }
		public bool IsTrialTransaction { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public UserModel User { get; set; }
	}
}
