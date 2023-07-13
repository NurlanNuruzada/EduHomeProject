using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel
{
    public class UpdateEventViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(40, ErrorMessage = "Title cannot exceed 40 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; } = null!;

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Event detail title is required.")]
        [StringLength(50, ErrorMessage = "Event detail title cannot exceed 50 characters.")]
        public string EventDetailTitle { get; set; } = null!;

        [Required(ErrorMessage = "Event detail description is required.")]
        [StringLength(900, ErrorMessage = "Event detail description cannot exceed 900 characters.")]
        public string EventDetailDescription { get; set; } = null!;

    }
}
