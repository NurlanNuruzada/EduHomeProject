using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;

public class CourseViewModel
{
    [Required, MaxLength(50)]
    public string? Title { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }

    public int CourseCatagoryId { get; set; }
    public CourseCatagory CourseCatagory { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    public int CourseDetailId { get; set; }
    [Required]
    public CourseDetail CourseDetail { get; set; } = null!;
    [Required]
    public int CourseFee { get; set; }

    [Required, StringLength(900)]
    public string AboutCourse { get; set; } = null!;

    [Required, StringLength(900)]
    public string HowToApply { get; set; } = null!;

    [Required, StringLength(900)]
    public string Certification { get; set; } = null!;
    [Required]
    public TimeSpan Duration { get; set; }  
    [Required]
    public DateTime Starts { get; set; }
    [Required]
    public TimeSpan ClassDuration { get; set; }
    public ICollection<Assesments> Assessments { get; set; }
    public ICollection<SkillLevel> SkillLevels { get; set; }
    public ICollection<Language> Languages { get; set; }
}
