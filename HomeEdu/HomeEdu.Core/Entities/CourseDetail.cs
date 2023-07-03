using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class CourseDetail : IEntity
{
    public int Id { get; set; }
    public int CourseFee { get; set; }

    [Required, MaxLength(900)]
    public string AboutCourse { get; set; } = null!;

    [Required, MaxLength(900)]
    public string HowToApply { get; set; } = null!;

    [Required, MaxLength(900)]
    public string Certification { get; set; } = null!;
    [Required]

    public DateTime Starts { get; set; } 
    [Required]

    public TimeSpan Duration { get; set; }
    public TimeSpan ClassDuration { get; set; }
    // One-to-many relationship with Course
    public ICollection<Course> Courses { get; set; }

    // One-to-many relationship with Language
    public ICollection<Language> Languages { get; set; } = new List<Language>();

    // One-to-many relationship with Assesments
    public ICollection<Assesments> Assesments { get; set; } = new List<Assesments>();

    // One-to-many relationship with SkillLevel
    public ICollection<SkillLevel> SkillLevels { get; set; } = new List<SkillLevel>();
}