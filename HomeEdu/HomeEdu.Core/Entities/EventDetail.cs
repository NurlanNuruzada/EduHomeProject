using HomeEdu.Core.Entities;
using HomeEdu.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EventDetail : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(300)]
    public string ImagePath { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(900)]
    public string Description { get; set; } = null!;

    // One-to-one relationship with Event
    public int EventId { get; set; }
    public Event Event { get; set; }

    // One-to-many relationship with Speaker
    public ICollection<Speaker> Speakers { get; set; }
}