using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.SpeakersViewModels
{
    public class CreateSpekerVM
    {
        [Required(ErrorMessage = "Image is required.")]
        public IFormFile Image { get; set; } = null!;
        [Required]
        [StringLength(50, ErrorMessage = "Name is requred")]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(50, ErrorMessage = "Position is requred")]
        public string Position { get; set; } = null!;
    }
}
