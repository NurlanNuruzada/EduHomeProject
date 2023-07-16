using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModels
{
    public class UpdateCourseViewModel
    {
        [Required, MaxLength(50)]
        public string? Title { get; set; }

        [Required, MaxLength(150)]
        public string? Description { get; set; }

        public int CourseCatagoryId { get; set; }
        public CourseCatagory? CourseCatagory { get; set; }
        public IFormFile? Image { get; set; } 
        public int CourseDetailId { get; set; }
        public CourseDetail? CourseDetail { get; set; }
        [Required]
        public int CourseFee { get; set; }
        [Required, MaxLength(5000)] 
        public string? CourseDescription { get; set; }
        [Required]
        public DateTime Duration { get; set; }
        [Required]
        public DateTime Starts { get; set; }
        [Required]
        public DateTime ClassDuration { get; set; }
    }
}
