using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AllForRent.Data.Enum;

namespace AllForRent.Models
{
    public class ProductCard
    {
        [Key]
        public int Id { get; set; }
        public string HeadTitle { get; set; }
        public string Description { get; set; } = string.Empty;
        public string SaleDescription { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        [ForeignKey("ProductCardImages")]
        public int ProductCardImagesId { get; set; }
        public ProductCardImages Image { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public RentTime RentTime { get; set; }
    }
}
