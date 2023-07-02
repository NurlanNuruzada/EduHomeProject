using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class Assesments : IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string assesments { get; set; } = null!;
    public int CourseDetailId { get; set; }
    public CourseDetail CourseDetail { get; set; } = null!;
}