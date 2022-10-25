using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SeaOfShops.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Настоящее имя")]
        public string? RealName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждаем пароль")]
        public string PasswordConfirm { get; set; }

        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
