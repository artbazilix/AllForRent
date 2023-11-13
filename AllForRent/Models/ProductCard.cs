using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AllForRent.Data.Enum;

namespace AllForRent.Models
{
    public class ProductCard
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal? Price { get; set; }
        public string? Image { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public RentTime RentTime { get; set; }
        //public string? Seller { get; set; }
        //у прайса был стринг
    }
}
