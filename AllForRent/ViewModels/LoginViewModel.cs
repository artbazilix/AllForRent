using System.ComponentModel.DataAnnotations;

namespace AllForRent.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email адрес")]
        [Required(ErrorMessage = "Введите электронную почту")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
