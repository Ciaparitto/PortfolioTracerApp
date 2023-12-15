namespace PortfolioApp.Models
{
	public class CurrencyModel
	{
		public bool Success { get; set; }
		public long Timestamp { get; set; }
		public bool? Historical { get; set; }
		public string Base { get; set; }
		public DateTime? Date { get; set; }
		public Dictionary<string, decimal> Rates { get; set; }
	}
}
