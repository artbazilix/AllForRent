namespace AllForRent.ViewModels
{
    public class CreateProductCardViewModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
