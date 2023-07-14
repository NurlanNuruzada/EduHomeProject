using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel
{
    public class UpdateTestimonialViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; } = null!;
        [Required, StringLength(50)]
        public string Surname { get; set; } = null!;
        [Required, StringLength(200)]
        public string Description { get; set; } = null!;
        [Required, StringLength(50)]
        public string Position { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
