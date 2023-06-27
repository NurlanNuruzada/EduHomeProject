using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;

public class BlogPostVM
{
    [Required, MaxLength(20)]
    public string? PostedBy { get; set; }
    [Required]
    public DateTime PostTime { get; set; }
    [Required, MaxLength(255)]
    public string? ImagePath { get; set; }
    public int CommentCount { get; set; }
    [MaxLength(255)]
    public string? Comment { get; set; }
    [Required]
    public int BlogCatagoryId { get; set; }
}
