namespace AllForRent.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? State {  get; set; }
        public string? Street { get; set; }
        public IFormFile? Image { get; set; }
    }
}
