using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Models;
using System;

namespace PortfolioApp
{
    public class AppDbContext : IdentityDbContext<UserModel>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<AssetModel> Assets { get; set; }
		public DbSet<TransactionModel> Transactions { get; set; }
		
	}
}
