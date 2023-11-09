using AllForRent.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace AllForRent.Models
{
    public class AppSeller
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RentTime RentTime { get; set; }

    }
}
