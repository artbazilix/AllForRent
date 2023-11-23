using System.ComponentModel.DataAnnotations;

namespace AllForRent.Models
{
    public class ProductCardImages
    {
        [Key]
        public int Id { get; set; }
        public string First { get; set; } = string.Empty;
        public string Second { get; set; } = string.Empty;
        public string Third { get; set; } = string.Empty;
        public string Fourth { get; set; } = string.Empty;
        public string Fifth { get; set; } = string.Empty;
    }
}
