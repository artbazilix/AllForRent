using System.ComponentModel.DataAnnotations;

namespace AllForRent.Models
{
    public class ProductCardImages
    {
        [Key]
        public int Id { get; set; }
        public string First { get; set; }
        public string? Second { get; set; }
        public string? Third { get; set; }
        public string? Fourth { get; set; }
        public string? Fifth { get; set; }
    }
}
