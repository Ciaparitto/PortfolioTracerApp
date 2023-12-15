namespace PortfolioApp.Models
{
	public class ConvertModel
	{
		public bool Success { get; set; }
		public QueryInfo Query { get; set; }
		public Info Info { get; set; }
		public decimal Result { get; set; }
	}

	public class QueryInfo
	{
		public string From { get; set; }
		public string To { get; set; }
		public decimal Amount { get; set; }
	}

	public class Info
	{
		public decimal Quote { get; set; }
		public long Timestamp { get; set; }
	}
}
