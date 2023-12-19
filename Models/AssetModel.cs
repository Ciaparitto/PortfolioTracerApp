using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioApp.Models
{
	public class AssetModel
	{
		[Key]
		public int id { get; set; }
		[Required]
		public string AssetCode { get; set; }
		[Required]
		public double Ammount{ get; set; }
		[Required]
		public string TypeOfAsset { get; set; }

		public DateTime AddedDate = DateTime.Now;
		[ForeignKey("UserId")]
		public string? UserId { get; set; }
		public UserModel? User {  get; set; }

	}
}
