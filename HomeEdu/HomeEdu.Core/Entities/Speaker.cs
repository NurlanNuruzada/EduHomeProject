using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class Speaker : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(300)]
    public string ImagePath { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public string Position { get; set; }

    // Many-to-one relationship with EventDetail
    public int EventDetailId { get; set; }
    public EventDetail EventDetail { get; set; }
}
