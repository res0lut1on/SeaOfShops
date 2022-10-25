using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SeaOfShops.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Настоящее имя")]
        public string? RealName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
