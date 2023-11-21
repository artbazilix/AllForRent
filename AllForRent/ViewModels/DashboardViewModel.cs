using AllForRent.Models;

namespace AllForRent.ViewModels
{
    public class DashboardViewModel
    {
        public List<ProductCard> ProductCards { get; set; }
        public decimal Balance { get; set; }
    }
}
