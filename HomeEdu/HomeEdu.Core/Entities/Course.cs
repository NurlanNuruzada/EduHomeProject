using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class Course: IEntity
{
    public int Id { get; set; }
    [Required,MaxLength(50)]
    public string? Title { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }
    public int CourseCatagoryId { get; set; }
    public CourseCatagory CourseCatagory { get; set; } = null!;
    [Required]
    public String ImagePath { get; set; } = null!;
    [Required]
    public CourseDetail CourseDetail { get; set; }=null!;
}