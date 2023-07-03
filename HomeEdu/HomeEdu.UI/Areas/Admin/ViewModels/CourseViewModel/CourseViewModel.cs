using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;

public class CourseViewModel
{
    [Required, MaxLength(50)]
    public string Title { get; set; }

    [Required, MaxLength(150)]
    public string Description { get; set; }

    public int CourseCatagoryId { get; set; }
    public CourseCatagory CourseCatagory { get; set; }

    [Required]
    public string ImagePath { get; set; }

    public int CourseDetailId { get; set; }
    public CourseDetail CourseDetail { get; set; }

    [Required]
    public int CourseFee { get; set; }

    [Required, MaxLength(900)]
    public string AboutCourse { get; set; }

    [Required, MaxLength(900)]
    public string HowToApply { get; set; }

    [Required, MaxLength(900)]
    public string Certification { get; set; }

    [Required]
    public DateTime Duration { get; set; }

    [Required]
    public DateTime Starts { get; set; }

    [Required]
    public DateTime ClassDuration { get; set; }

}
