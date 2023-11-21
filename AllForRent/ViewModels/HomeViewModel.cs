using AllForRent.Models;

namespace AllForRent.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductCard> ProductCards { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
		public IEnumerable<ProductCardImages> Images { get; set; }
	}
}
