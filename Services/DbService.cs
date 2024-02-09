using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Components.Services.Interfaces;
using PortfolioApp.Models;
using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class DbService : IDbService
	{
		private readonly AppDbContext _Context;
		private readonly IUserService _userService;
		public DbService(AppDbContext context,IUserService userService) 
		{
			_Context = context;
			_userService = userService;

		}
		public async Task AddAssetToDb(AssetModel model)
		{
			await _Context.Assets.AddAsync(model);
			await _Context.SaveChangesAsync();
		}

		public async Task<Dictionary<string, string>> GetMetalDict()
		{
			Dictionary<string, string> MetalsDict = new Dictionary<string, string>
			{
			{ "XAU", "Gold" },
			{ "XAG", "Silver" },
			{ "XPT", "Platinum" },
			{ "XPD", "Palladium" },
			};
			return MetalsDict;
		}
		public async Task<Dictionary<string, string>> GetCurrencyDict()
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
			{"PLN","Polish Zloty"}
			};
			return CurrenciesDict;
		}
		public async Task<Dictionary<string, string>> GetCryptoCurrencyDict()
		{
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
			return CryptoDict;
		}
		public async Task<List<TransactionModel>> GetUserTransactions()
		{
			var USER = await _userService.GetLoggedUser();
			var List = await _Context.Transactions.Where(x => x.UserId == USER.Id).OrderByDescending(x => x.date).ToListAsync();
			return List;
		
			
		}
		public async Task AddTransactionToDb(TransactionModel model)
		{
			await _Context.Transactions.AddAsync(model);
			await _Context.SaveChangesAsync();
		}

	}
}
