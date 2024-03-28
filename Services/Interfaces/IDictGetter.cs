namespace PortfolioApp.Services.Interfaces
{
	public interface IDictGetter
	{
		public Task<Dictionary<string, string>> GetMetalDict();
		public Task<Dictionary<string, string>> GetCurrencyDict();
		public Task<Dictionary<string, string>> GetCryptoCurrencyDict();
	}
}
