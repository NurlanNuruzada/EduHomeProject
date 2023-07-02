using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class Language : IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Langugage { get; set; } = null!;
    public int EventDetailId { get; set; }
    public CourseCatagory EventDetail { get; set; } = null!;
}