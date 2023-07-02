using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class CourseDetail : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(900)]
    public string AboutCourse { get; set; } = null!;

    [Required, MaxLength(900)]
    public string HowToApply { get; set; } = null!;

    [Required, MaxLength(900)]
    public string  Certfication{ get; set; } = null!;
    [Required]

    // One-to-one relationship with Event
    public int CourseId { get; set; }
    public Event Course { get; set; }

    // One-to-many relationship with Speaker
    public ICollection<Language> languages { get; set; }
    public ICollection<Assesments> Assesments { get; set; }
    public ICollection<SkillLevel> SkillLevels { get; set; }
}