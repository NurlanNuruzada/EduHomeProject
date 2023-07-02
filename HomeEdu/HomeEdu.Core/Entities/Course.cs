using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class Course: IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string? Title { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }

    // Many-to-one relationship with CourseCatagory
    public int CourseCatagoryId { get; set; }
    public CourseCatagory CourseCatagory { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    // Many-to-one relationship with CourseDetail
    public int CourseDetailId { get; set; }
    public CourseDetail CourseDetail { get; set; } = null!;

}