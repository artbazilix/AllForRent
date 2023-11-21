using AllForRent.Interfaces;
using AllForRent.Models;

namespace AllForRent.Services
{
    public class SearchService
    {
        private readonly IProductCardRepository _productRepository;

        public SearchService(IProductCardRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductCard> SearchProducts(string name)
        {
            return _productRepository.SearchByName(name);
        }
    }
}
