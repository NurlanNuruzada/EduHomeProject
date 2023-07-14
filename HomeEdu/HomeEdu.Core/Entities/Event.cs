using HomeEdu.Core.Entities;
using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class Event : IEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = null!;
    public EventDetail EventDetail { get; set; }
}