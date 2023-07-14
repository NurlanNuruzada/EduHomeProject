using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel
{
    public class UpdateEventViewModel
    {
        [StringLength(40, ErrorMessage = "Title cannot exceed 40 characters.")]
        public string Title { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; } = null!;
        public IFormFile? Image { get; set; }

        public string EventDetailTitle { get; set; } = null!;

        [StringLength(900, ErrorMessage = "Event detail description cannot exceed 900 characters.")]
        public string EventDetailDescription { get; set; } = null!;

    }
}
