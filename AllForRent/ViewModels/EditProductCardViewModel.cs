namespace AllForRent.ViewModels
{
    public class EditProductCardViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? URL { get; set; }
    }
}
