using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllForRent.Models
{
	public class Purchase
	{
		[Key]
		public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
		public string? UserEmail { get; set; }
		public string? ProductName { get; set; }
		public string? ProductDescription { get; set; }
		public DateTime PurchaseTime { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal ProductPrice { get; set; }
	}
}
