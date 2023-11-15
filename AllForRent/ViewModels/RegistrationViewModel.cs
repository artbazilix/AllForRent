using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AllForRent.ViewModels
{
    public class RegistrationViewModel
    {
        [Display(Name = "Email адрес")]
        [Required(ErrorMessage = "Введите электронную почту")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Введите электронную почту")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
