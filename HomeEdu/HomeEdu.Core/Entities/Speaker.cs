using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class Speaker : IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(300)]
    public string ImagePath { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    public string Position { get; set; }
    public int EventDetailId { get; set; }
    public EventDetail EventDetail { get; set; }
}
