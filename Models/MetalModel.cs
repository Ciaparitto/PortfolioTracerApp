namespace PortfolioApp.Models
{
	public class MetalModel
	{
		public DateTime? date { get; set; }
		public long timestamp { get; set; }
		public string metal { get; set; }
		public string exchange { get; set; }
		public string currency { get; set; }
		public double price { get; set; }
		public double prev_close_price { get; set; }
		public double ch { get; set; }
		public double chp { get; set; }
		public double price_gram_24k { get; set; }
		public double price_gram_22k { get; set; }
		public double price_gram_21k { get; set; }
		public double price_gram_20k { get; set; }
		public double price_gram_18k { get; set; }
		public double price_gram_16k { get; set; }
		public double price_gram_14k { get; set; }
		public double price_gram_10k { get; set; }
	}
}
