using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AllForRent.Models
{
    public class ProductCard
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Seller { get; set; }
        public string? ProductPrice { get; set; }
        public string? Image { get; set; }
    }
}
