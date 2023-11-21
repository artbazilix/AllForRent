using AllForRent.Models;
using System.ComponentModel.DataAnnotations;

public class EditProductCardViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите заголовок")]
    [StringLength(60, ErrorMessage = "Название не должно превышать 60 символов.")]
    public string? HeadTitle { get; set; }

    [Required(ErrorMessage = "Придумайте описание.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите цену.")]
    [Range(0, 9999999, ErrorMessage = "Пожалуйста, введите действительную цену.")]
    public decimal? Price { get; set; }

    public List<IFormFile>? Images { get; set; }
    public List<string>? ImageUrls { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}

