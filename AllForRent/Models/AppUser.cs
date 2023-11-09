using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AllForRent.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
