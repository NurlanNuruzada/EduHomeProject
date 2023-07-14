using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(40)]
        public string Title { get; set; } = null!;
        [Required, StringLength(200)]
        public string Description { get; set; } = null!;
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}
