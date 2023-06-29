using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities;

public class BlogCatagory:IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(30)]
    public string? Catagory { get; set; }
    public ICollection<Blog>? Blogs { get; set; }
}
