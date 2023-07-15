using HomeEdu.Core.Interfaces;

namespace HomeEdu.Core.Entities;

public class Blog : IEntity
{
    public int Id { get; set; }
    public string? PostedBy { get; set; }
    public DateTime PostTime { get; set; }
    public string? ImagePath { get; set; }
    public int CommentCount { get; set; }
    public string? Comment { get; set; }
    public string? Title { get; set; }
    public int BlogCatagoryId { get; set; }
    public BlogCatagory BlogCatagory { get; set; } = null!;
}
