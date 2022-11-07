using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SeaOfShops.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Настоящее имя")]
        public string? RealName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
