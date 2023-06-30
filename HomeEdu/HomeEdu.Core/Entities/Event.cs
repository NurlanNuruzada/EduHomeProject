﻿using HomeEdu.Core.Entities;
using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class Event : IEntity
{
    public int Id { get; set; }

    [Required, MaxLength(40)]
    public string Title { get; set; } = null!;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required, MaxLength(100)]
    public string Location { get; set; } = null!;

    [Required]
    public EventDetail EventDetail { get; set; }
}