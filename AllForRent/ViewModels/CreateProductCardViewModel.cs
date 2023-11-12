namespace AllForRent.ViewModels
{
    public class CreateProductCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Seller { get; set; }
        public string ProductPrice { get; set; }
        public IFormFile Image { get; set; }
    }
}
