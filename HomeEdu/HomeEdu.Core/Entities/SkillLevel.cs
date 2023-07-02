using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class SkillLevel : IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string skillLevel { get; set; } = null!;
    public int EventDetailId { get; set; }
    public CourseCatagory EventDetail { get; set; } = null!;
}